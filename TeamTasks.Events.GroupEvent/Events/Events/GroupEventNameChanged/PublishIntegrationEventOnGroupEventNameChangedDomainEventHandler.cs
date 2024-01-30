using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Events;
using TeamTasks.RabbitMq.Messaging;

namespace TeamTasks.Events.GroupEvent.Events.Events.GroupEventNameChanged;

/// <summary>
/// Represents the <see cref="GroupEventNameChangedDomainEvent"/> class.
/// </summary>
public sealed class PublishIntegrationEventOnGroupEventNameChangedDomainEventHandler
    : IDomainEventHandler<GroupEventNameChangedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnGroupEventNameChangedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnGroupEventNameChangedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async Task Handle(GroupEventNameChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        _integrationEventPublisher.Publish(new GroupEventNameChangedIntegrationEvent(notification));

        await Task.CompletedTask;
    }
}