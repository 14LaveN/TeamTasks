using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Events;
using TeamTasks.RabbitMq.Messaging;

namespace TeamTasks.Events.GroupEvent.Events.Events.GroupEventCancelled;

/// <summary>
/// Represents the <see cref="GroupEventCancelledDomainEvent"/> class.
/// </summary>
internal sealed class PublishIntegrationEventOnGroupEventCancelledDomainEventHandler
    : IDomainEventHandler<GroupEventCancelledDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnGroupEventCancelledDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnGroupEventCancelledDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async Task Handle(GroupEventCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.Publish(new GroupEventCancelledIntegrationEvent(notification));

        await Task.CompletedTask;
    }
}