using MediatR;
using TeamTasks.Application.Core.Abstractions.Common;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.GroupEvent.Data.Interfaces;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Domain.ValueObjects;

namespace TeamTasks.Events.GroupEvent.Events.Commands.CreateGroupEvent;

/// <summary>
/// Represents the <see cref="CreateGroupEventCommand"/> handler.
/// </summary>
internal sealed class CreateGroupEventCommandHandler : ICommandHandler<CreateGroupEventCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUnitOfWork<Domain.Entities.GroupEvent> _unitOfWork;
    private readonly IDateTime _dateTime;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGroupEventCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="mediator">The mediator.</param>
    public CreateGroupEventCommandHandler(
        IUserRepository userRepository,
        IGroupEventRepository groupEventRepository,
        IUnitOfWork<Domain.Entities.GroupEvent> unitOfWork,
        IDateTime dateTime,
        IMediator mediator)
    {
        _userRepository = userRepository;
        _groupEventRepository = groupEventRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(CreateGroupEventCommand request, CancellationToken cancellationToken)
    {
        if (request.DateTimeUtc <= _dateTime.UtcNow)
        {
            return Result.Failure(DomainErrors.GroupEvent.DateAndTimeIsInThePast);
        }

        Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId);

        if (maybeUser.HasNoValue)
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        Maybe<Category> maybeCategory = Category.FromValue(request.CategoryId);

        if (maybeCategory.HasNoValue)
        {
            return Result.Failure(DomainErrors.Category.NotFound);
        }

        Result<Name> nameResult = Name.Create(request.Name);

        if (nameResult.IsFailure)
        {
            return Result.Failure(nameResult.Error);
        }

        var groupEvent = Domain.Entities.GroupEvent.Create(maybeUser.Value, nameResult.Value, maybeCategory.Value, request.DateTimeUtc);
        
        await _groupEventRepository.Insert(groupEvent);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}