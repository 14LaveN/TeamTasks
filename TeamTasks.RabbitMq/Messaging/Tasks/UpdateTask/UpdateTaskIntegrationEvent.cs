using System.Text.Json.Serialization;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Events;

namespace TeamTasks.RabbitMq.Messaging.Tasks.UpdateTask;

/// <summary>
/// Represents the event that is raised when the task is done.
/// </summary>
public sealed class UpdateTaskIntegrationEvent
    : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTaskIntegrationEvent"/> class.
    /// </summary>
    /// <param name="domainEvent">The <see cref="DoneTaskDomainEvent"/> class.></param>
    internal UpdateTaskIntegrationEvent(UpdateTaskDomainEvent domainEvent) =>
        TaskId = domainEvent.Task.Id;

    [JsonConstructor]
    private UpdateTaskIntegrationEvent(Guid taskId) => 
        TaskId = taskId;
    
    /// <summary>
    /// Gets the task identifier.
    /// </summary>
    public Guid TaskId { get; }
}