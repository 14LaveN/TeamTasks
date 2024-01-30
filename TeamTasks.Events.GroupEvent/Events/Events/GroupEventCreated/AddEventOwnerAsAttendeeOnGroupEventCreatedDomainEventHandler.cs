using TeamTasks.Database.Attendee.Data.Interfaces;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Events;


namespace TeamTasks.Events.GroupEvent.Events.Events.GroupEventCreated;

/// <summary>
/// Represents the <see cref="GroupEventCreatedDomainEvent"/> handler.
/// </summary>
internal sealed class AddEventOwnerAsAttendeeOnGroupEventCreatedDomainEventHandler : IDomainEventHandler<GroupEventCreatedDomainEvent>
{
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork<Domain.Entities.GroupEvent> _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddEventOwnerAsAttendeeOnGroupEventCreatedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="attendeeRepository">The attendee repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public AddEventOwnerAsAttendeeOnGroupEventCreatedDomainEventHandler(
        IAttendeeRepository attendeeRepository,
        IUnitOfWork<Domain.Entities.GroupEvent> unitOfWork)
    {
        _attendeeRepository = attendeeRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task Handle(GroupEventCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Attendee attendee = notification.GroupEvent.GetOwner();

        await _attendeeRepository.Insert(attendee);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}