using System.Threading;
using System.Threading.Tasks;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Events;
using TeamTasks.Domain.Events;
using TeamTasks.Micro.Identity.Invitations.Events.InvitationAccepted;
using TeamTasks.RabbitMq.Messaging;

namespace TeamTasks.Application.Invitations.Events.InvitationAccepted;

/// <summary>
/// Represents the <see cref="InvitationAcceptedDomainEvent"/> handler.
/// </summary>
internal sealed class PublishIntegrationEventOnInvitationAcceptedDomainEventHandler
    : IDomainEventHandler<InvitationAcceptedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnInvitationAcceptedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnInvitationAcceptedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async Task Handle(InvitationAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.Publish(new InvitationAcceptedIntegrationEvent(notification));

        await Task.CompletedTask;
    }
}