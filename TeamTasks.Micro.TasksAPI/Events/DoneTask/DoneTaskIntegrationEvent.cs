using System.Text.Json.Serialization;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Events;

namespace TeamTasks.Micro.TasksAPI.Events.DoneTask;

/// <summary>
/// Represents the event that is raised when the task is done.
/// </summary>
public sealed class DoneTaskIntegrationEvent
    : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoneTaskIntegrationEvent"/> class.
    /// </summary>
    /// <param name="domainEvent">The <see cref="DoneTaskDomainEvent"/> class.></param>
    internal DoneTaskIntegrationEvent(DoneTaskDomainEvent domainEvent) =>
        TaskId = domainEvent.Task.Id;

    [JsonConstructor]
    private DoneTaskIntegrationEvent(Guid taskId) => 
        TaskId = taskId;
    
    /// <summary>
    /// Gets the task identifier.
    /// </summary>
    public Guid TaskId { get; }
}