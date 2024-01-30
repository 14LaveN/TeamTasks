using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Events;
using TeamTasks.RabbitMq.Messaging;

namespace TeamTasks.Micro.Identity.Invitations.Events.InvitationSent;

/// <summary>
/// Represents the <see cref="InvitationRejectedDomainEvent"/> handler.
/// </summary>
internal sealed class PublishIntegrationEventOnInvitationSentDomainEventHandler : IDomainEventHandler<InvitationSentDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnInvitationSentDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnInvitationSentDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async Task Handle(InvitationSentDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.Publish(new InvitationSentIntegrationEvent(notification));

        await Task.CompletedTask;
    }
}