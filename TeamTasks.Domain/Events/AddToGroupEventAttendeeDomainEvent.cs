using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Domain.Events;

/// <summary>
/// Represents the event that is raised when add to group event attendee.
/// </summary>
public sealed class AddToGroupEventAttendeeDomainEvent
    : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddToGroupEventAttendeeDomainEvent"/> class.
    /// </summary>
    /// <param name="groupEvent">The group event.</param>
    /// <param name="attendee">The attendee.</param>
    internal AddToGroupEventAttendeeDomainEvent(GroupEvent groupEvent, User attendee)
    {
        GroupEvent = groupEvent;
        Attendee = attendee;
    }

    /// <summary>
    /// Gets the group event.
    /// </summary>
    public GroupEvent GroupEvent { get; }
    
    /// <summary>
    /// Gets the attendee.
    /// </summary>
    public User Attendee { get; }
}