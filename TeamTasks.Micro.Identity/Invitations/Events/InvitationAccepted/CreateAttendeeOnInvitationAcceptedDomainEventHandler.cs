using MediatR;
using TeamTasks.Database.Attendee.Data.Interfaces;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Events;
using TeamTasks.Micro.Identity.Attendees.Events.AttendeeCreated;

namespace TeamTasks.Micro.Identity.Invitations.Events.InvitationAccepted;

/// <summary>
/// Represents the <see cref="InvitationAcceptedDomainEvent"/> handler.
/// </summary>
internal sealed class CreateAttendeeOnInvitationAcceptedDomainEventHandler : IDomainEventHandler<InvitationAcceptedDomainEvent>
{
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork<Attendee> _unitOfWork;
    private readonly IPublisher _publisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAttendeeOnInvitationAcceptedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="attendeeRepository">The attendee repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="publisher">The publisher.</param>
    public CreateAttendeeOnInvitationAcceptedDomainEventHandler(
        IAttendeeRepository attendeeRepository,
        IUnitOfWork<Attendee> unitOfWork,
        IPublisher publisher)
    {
        _attendeeRepository = attendeeRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    /// <inheritdoc />
    public async Task Handle(InvitationAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        var attendee = new Attendee(notification.Invitation);
            
        await _attendeeRepository.Insert(attendee);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new AttendeeCreatedEvent(attendee.Id), cancellationToken);
    }
}