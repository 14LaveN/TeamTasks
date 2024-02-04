using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Domain.Events;

/// <summary>
/// Represents the event that is raised when the task updated.
/// </summary>
public sealed class UpdateTaskDomainEvent
    : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTaskDomainEvent"/> class.
    /// </summary>
    /// <param name="task">The task entity.</param>
    /// <param name="user">The user.</param>
    internal UpdateTaskDomainEvent(
        TaskEntity task,
        User user) =>
        (Task, User) = 
        (task,user);

    /// <summary>
    /// Gets the task entity.
    /// </summary>
    public TaskEntity Task { get; }

    /// <summary>
    /// Gets the user entity.
    /// </summary>
    public User User { get; }
}