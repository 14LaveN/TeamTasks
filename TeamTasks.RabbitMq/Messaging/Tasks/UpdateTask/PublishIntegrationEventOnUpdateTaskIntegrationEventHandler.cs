using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Events;

namespace TeamTasks.RabbitMq.Messaging.Tasks.UpdateTask;

public sealed class PublishIntegrationEventOnUpdateTaskIntegrationEventHandler
    : IDomainEventHandler<UpdateTaskDomainEvent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="integrationEventPublisher"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnUpdateTaskIntegrationEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;
    
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public async Task Handle(UpdateTaskDomainEvent notification, CancellationToken cancellationToken) =>
        await _integrationEventPublisher.Publish(new UpdateTaskIntegrationEvent(notification));
}