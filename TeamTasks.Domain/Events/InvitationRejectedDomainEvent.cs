using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Domain.Events
{
    /// <summary>
    /// Represents the event that is raised when an invitation is rejected.
    /// </summary>
    public sealed class InvitationRejectedDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvitationRejectedDomainEvent"/> class.
        /// </summary>
        /// <param name="invitation">The invitation.</param>
        internal InvitationRejectedDomainEvent(Invitation invitation) => Invitation = invitation;

        /// <summary>
        /// Gets the invitation.
        /// </summary>
        public Invitation Invitation { get; }
    }
}