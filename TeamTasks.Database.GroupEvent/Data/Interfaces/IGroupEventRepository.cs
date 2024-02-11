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
    /// Inserts the specified group event to the database.
    /// </summary>
    /// <param name="groupEvent">The group event to be inserted to the database.</param>
    Task Insert(Domain.Entities.GroupEvent groupEvent);
    
    /// <summary>
    /// Gets the specified number of unprocessed group events, if they exist.
    /// </summary>
    /// <param name="take">The number of group events to take.</param>
    /// <returns>The specified number of unprocessed group events, if they exist.</returns>
    Task<IReadOnlyCollection<Domain.Entities.GroupEvent>> GetUnprocessedAsync(int take);

    /// <summary>
    /// Gets the group event with the specified name.
    /// </summary>
    /// <param name="name">The group event name.</param>
    /// <returns>The maybe instance that may contain the group event with the specified name.</returns>
    Task<Maybe<Domain.Entities.GroupEvent>> GetGroupEventByName(string name);
}