using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.GroupEvent.Data.Interfaces;

/// <summary>
/// Represents the group event repository interface.
/// </summary>
public interface IGroupEventRepository
{
    /// <summary>
    /// Gets the group event with the specified identifier.
    /// </summary>
    /// <param name="groupEventId">The group event identifier.</param>
    /// <returns>The maybe instance that may contain the group event with the specified identifier.</returns>
    Task<Maybe<Domain.Entities.GroupEvent>> GetByIdAsync(Guid groupEventId);

    /// <summary>
    /// Gets the distinct group events for the specified attendees.
    /// </summary>
    /// <param name="attendees">The attendees to get the group events for.</param>
    /// <returns>The readonly collection of group events with the specified identifiers.</returns>
    Task<IReadOnlyCollection<Domain.Entities.GroupEvent>> GetForAttendeesAsync(IReadOnlyCollection<Attendee> attendees);

    /// <summary>
    /// Inserts the specified group event to the database.
    /// </summary>
    /// <param name="groupEvent">The group event to be inserted to the database.</param>
    Task Insert(Domain.Entities.GroupEvent groupEvent);
}