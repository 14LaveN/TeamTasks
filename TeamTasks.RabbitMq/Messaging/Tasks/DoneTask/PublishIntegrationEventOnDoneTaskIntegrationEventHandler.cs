using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Events;

namespace TeamTasks.RabbitMq.Messaging.Tasks.DoneTask;

public sealed class PublishIntegrationEventOnDoneTaskIntegrationEventHandler
    : IDomainEventHandler<DoneTaskDomainEvent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnDoneTaskIntegrationEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnDoneTaskIntegrationEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;
    
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public async Task Handle(DoneTaskDomainEvent notification, CancellationToken cancellationToken) =>
        await _integrationEventPublisher.Publish(new DoneTaskIntegrationEvent(notification));
}