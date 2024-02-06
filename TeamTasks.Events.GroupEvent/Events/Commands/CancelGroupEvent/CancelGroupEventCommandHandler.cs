using TeamTasks.Application.Core.Abstractions.Common;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.GroupEvent.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;

namespace TeamTasks.Events.GroupEvent.Events.Commands.CancelGroupEvent;

/// <summary>
/// Represents the <see cref="CancelGroupEventCommand"/> handler.
/// </summary>
internal sealed class CancelGroupEventCommandHandler : ICommandHandler<CancelGroupEventCommand, Result>
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUnitOfWork<Domain.Entities.GroupEvent> _unitOfWork;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelGroupEventCommandHandler"/> class.
    /// </summary>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    public CancelGroupEventCommandHandler(
        IGroupEventRepository groupEventRepository,
        IUnitOfWork<Domain.Entities.GroupEvent> unitOfWork,
        IDateTime dateTime)
    {
        _groupEventRepository = groupEventRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(CancelGroupEventCommand request, CancellationToken cancellationToken)
    {
        Maybe<Domain.Entities.GroupEvent> maybeGroupEvent = await _groupEventRepository.GetByIdAsync(request.GroupEventId);

        if (maybeGroupEvent.HasNoValue)
        {
            return Result.Failure(DomainErrors.GroupEvent.NotFound);
        }

        Domain.Entities.GroupEvent groupEvent = maybeGroupEvent.Value;

        if (groupEvent.UserId != request.UserId)
        {
            return Result.Failure(DomainErrors.User.InvalidPermissions);
        }

        groupEvent.Processed = false;

        Result result = groupEvent.Cancel(_dateTime.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}