using Newtonsoft.Json;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Events;

namespace TeamTasks.Micro.TasksAPI.Events.CreateTask;

/// <summary>
/// Represents the event that is raised when a task is created.
/// </summary>
public sealed class TaskCreatedIntegrationEvent
    : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskCreatedIntegrationEvent"/> class.
    /// </summary>
    /// <param name="domainEvent">The <see cref="TaskCreatedDomainEvent"/>.</param>
    internal TaskCreatedIntegrationEvent(TaskCreatedDomainEvent domainEvent) => 
        TaskId = domainEvent.Task.Id;

    [JsonConstructor]
    private TaskCreatedIntegrationEvent(Guid taskId) =>
        TaskId = taskId;
    
    /// <summary>
    /// Gets the task identifier.
    /// </summary>
    public Guid TaskId { get; }
}