using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Events;
using TeamTasks.RabbitMq.Messaging;

namespace TeamTasks.Micro.TasksAPI.Events.CreateTask;

/// <summary>
/// Represents the <see cref="PublishIntegrationEventOnTaskCreatedDomainEventHandler"/> class.
/// </summary>
public sealed class PublishIntegrationEventOnTaskCreatedDomainEventHandler
    : IDomainEventHandler<TaskCreatedDomainEvent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnTaskCreatedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnTaskCreatedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;
    
    private readonly IIntegrationEventPublisher _integrationEventPublisher;
    
    public async Task Handle(TaskCreatedDomainEvent notification, CancellationToken cancellationToken) =>
        await _integrationEventPublisher.Publish(new TaskCreatedIntegrationEvent(notification));
}