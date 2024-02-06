using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Helpers.JWT;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.GroupEvent.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Exceptions;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;

namespace TeamTasks.Events.GroupEvent.Events.Commands.AddToGroupEventAttendee;

/// <summary>
/// Represents the <see cref="AddToGroupEventAttendeeCommandHandler"/> class.
/// </summary>
public sealed class AddToGroupEventAttendeeCommandHandler
    : ICommandHandler<AddToGroupEventAttendeeCommand, IBaseResponse<Result>>
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly ILogger<AddToGroupEventAttendeeCommandHandler> _logger;
    private readonly IUnitOfWork<Domain.Entities.GroupEvent> _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddToGroupEventAttendeeCommandHandler"/> class.
    /// </summary>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="logger">The logger.</param>
    public AddToGroupEventAttendeeCommandHandler(
        IGroupEventRepository groupEventRepository,
        IUnitOfWork<Domain.Entities.GroupEvent> unitOfWork,
        IUserIdentifierProvider userIdentifierProvider,
        ILogger<AddToGroupEventAttendeeCommandHandler> logger)
    {
        _groupEventRepository = groupEventRepository;
        _unitOfWork = unitOfWork;
        _userIdentifierProvider = userIdentifierProvider;
        _logger = logger;
    }
    
    //TODO Create the add person to company handler this AddToGroupEventAttendeeCommandHandler class.
    
    /// <inheritdoc />
    public async Task<IBaseResponse<Result>> Handle(
        AddToGroupEventAttendeeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for add to group event attendee - {request.Attendee} {DateTime.UtcNow}");
            
            if (_userIdentifierProvider.UserId == Guid.Empty)
            {
                _logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }
            
            Maybe<Domain.Entities.GroupEvent> maybeGroupEvent = await _groupEventRepository
                .GetByIdAsync(request.GroupEventId);

            if (maybeGroupEvent.HasNoValue)
            {
                _logger.LogWarning($"Group event with same identifier - {request.GroupEventId} not found.");
                throw new NotFoundException(nameof(DomainErrors.GroupEvent.NotFound), DomainErrors.GroupEvent.NotFound);
            }

            Domain.Entities.GroupEvent groupEvent = maybeGroupEvent.Value;
            
            groupEvent.Attendees.Add(request.Attendee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation($"Add to group event attendee - {groupEvent.Name} {groupEvent.CreatedOnUtc}");

            return new BaseResponse<Result>
            {
                Data = Result.Success(),
                StatusCode = StatusCode.Ok,
                Description = "Add to group event attendee"
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[AddToGroupEventAttendeeCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = StatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}