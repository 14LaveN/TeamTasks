using TeamTasks.Application.Core.Abstractions.Common;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.PersonalEvent.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;

namespace TeamTasks.Events.PersonalEvent.Events.Commands.CancelPersonalEvent;

/// <summary>
/// Represents the <see cref="CancelPersonalEventCommand"/> handler.
/// </summary>
internal sealed class CancelPersonalEventCommandHandler : ICommandHandler<CancelPersonalEventCommand, Result>
{
    private readonly IPersonalEventRepository _personalEventRepository;
    private readonly IUnitOfWork<Domain.Entities.PersonalEvent> _unitOfWork;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelPersonalEventCommandHandler"/> class.
    /// </summary>
    /// <param name="personalEventRepository">The personal event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    public CancelPersonalEventCommandHandler(
        IPersonalEventRepository personalEventRepository,
        IUnitOfWork<Domain.Entities.PersonalEvent> unitOfWork,
        IDateTime dateTime)
    {
        _personalEventRepository = personalEventRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(CancelPersonalEventCommand request, CancellationToken cancellationToken)
    {
        Maybe<Domain.Entities.PersonalEvent> maybePersonalEvent = await _personalEventRepository.GetByIdAsync(request.PersonalEventId);

        if (maybePersonalEvent.HasNoValue)
        {
            return Result.Failure(DomainErrors.PersonalEvent.NotFound);
        }

        Domain.Entities.PersonalEvent personalEvent = maybePersonalEvent.Value;

        if (personalEvent.UserId != request.UserId)
        {
            return Result.Failure(DomainErrors.User.InvalidPermissions);
        }

        Result result = personalEvent.Cancel(_dateTime.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
            
        return await Result.Success();
    }
}