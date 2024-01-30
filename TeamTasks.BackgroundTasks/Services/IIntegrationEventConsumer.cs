using TeamTasks.Application.Core.Abstractions.Messaging;

namespace TeamTasks.BackgroundTasks.Services;

/// <summary>
/// Represents the integration event consumer interface.
/// </summary>
internal interface IIntegrationEventConsumer
{
    /// <summary>
    /// Consumes the incoming the specified integration event.
    /// </summary>
    Task Consume(IIntegrationEvent? integrationEvent);
}