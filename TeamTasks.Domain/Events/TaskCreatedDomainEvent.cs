using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Domain.Events;

/// <summary>
/// Represents the event that is raised when a task is created.
/// </summary>
public sealed class TaskCreatedDomainEvent 
    : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskCreatedDomainEvent"/> class.
    /// </summary>
    /// <param name="task">The task entity.</param>
    internal TaskCreatedDomainEvent(TaskEntity task) => 
        Task = task;

    /// <summary>
    /// Gets the user.
    /// </summary>
    public TaskEntity Task { get; }
}