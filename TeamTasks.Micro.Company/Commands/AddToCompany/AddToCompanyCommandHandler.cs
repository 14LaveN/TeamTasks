using System.Windows.Input;
using MediatR;
using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Helpers.JWT;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Company.Data.Interfaces;
using TeamTasks.Database.GroupEvent.Data.Interfaces;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Exceptions;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Events.GroupEvent.Events.Commands.AddToGroupEventAttendee;
using TeamTasks.Events.GroupEvent.Events.Commands.CreateGroupEvent;

namespace TeamTasks.Micro.Company.Commands.AddToCompany;

/// <summary>
/// Represents the <see cref="AddToCompanyCommandHandler"/> class.
/// </summary>
public sealed class AddToCompanyCommandHandler
    : ICommandHandler<AddToCompanyCommand, IBaseResponse<Result>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ILogger<AddToCompanyCommandHandler> _logger;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;
    private readonly ISender _sender;
    private readonly IGroupEventRepository _groupEventRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddToCompanyCommandHandler"/> class.
    /// </summary>
    /// <param name="companyRepository">The company repository.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="groupEventRepository">The group event repository.</param>
    public AddToCompanyCommandHandler(
        ICompanyRepository companyRepository,
        ILogger<AddToCompanyCommandHandler> logger,
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository,
        ISender sender,
        IGroupEventRepository groupEventRepository)
    {
        _companyRepository = companyRepository;
        _logger = logger;
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
        _sender = sender;
        _groupEventRepository = groupEventRepository;
    }
    
    /// <inheritdoc />
    public async Task<IBaseResponse<Result>> Handle(AddToCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for add to company - {request.CompanyId} the user - {request.UserId} {DateTime.UtcNow}");

            var company = await _companyRepository
                .GetByIdAsync(request.CompanyId);
            
            if (company.HasNoValue)
            {
                _logger.LogWarning($"Your company not found by id - {request.CompanyId}");

                throw new NotFoundException(nameof(DomainErrors.Company.NotFound), DomainErrors.Company.NotFound);
            }
            
            if (_userIdentifierProvider.UserId == Guid.Empty && _userIdentifierProvider.UserId == company.Value.AuthorId)
            {
                _logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }

            var maybeUser = await _userRepository
                .GetByIdAsync(request.UserId);

            User user = maybeUser.Value;
            
            //The user identifier - author identifier and UserId - attendee identifier.
            string groupEventName = $"CreateGroupEvent-{_userIdentifierProvider.UserId}-{request.UserId}";
            
            var createGrEventResult = await _sender.Send(new CreateGroupEventCommand(
                _userIdentifierProvider.UserId,
                groupEventName,
                1,
                DateTime.UtcNow
                ), cancellationToken);
            
            if (createGrEventResult.IsFailure)
            {
                _logger.LogWarning(DomainErrors.GroupEvent.NotFound + $"by the id-{company.Value.Id}");
                throw new Exception(DomainErrors.GroupEvent.NotFound);
            }

            Maybe<GroupEvent> groupEvent = await _groupEventRepository
                    .GetGroupEventByName(groupEventName);

            if (groupEvent.HasNoValue)
            {
                _logger.LogWarning($"Your group event not found by name - {groupEventName}");

                throw new NotFoundException(nameof(DomainErrors.GroupEvent.NotFound), DomainErrors.GroupEvent.NotFound);
            }
            
            var addToGroupEventAttendeeResult = await _sender.Send(new AddToGroupEventAttendeeCommand(
                groupEvent.Value.Id,
                user), cancellationToken);

            if (addToGroupEventAttendeeResult.Data.GetAwaiter().GetResult().IsFailure)
            {
                _logger.LogWarning(DomainErrors.GroupEvent.NotFound + $"by the id-{company.Value.Id}");
                throw new Exception(DomainErrors.GroupEvent.NotFound);
            }
            
            //TODO work with this handler.
            
            _logger.LogInformation($"Add to company person- {user.UserName} {groupEvent.Value.Name}");

            return new BaseResponse<Result>
            {
                Data = Result.Success(),
                StatusCode = StatusCode.Ok,
                Description = "Add to company person"
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[DoneTaskCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = StatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}