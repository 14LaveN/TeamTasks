﻿using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.BackgroundTasks.Services;
using TeamTasks.RabbitMq.Messaging.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TeamTasks.BackgroundTasks.Tasks;

internal  sealed class IntegrationEventConsumerBackgroundService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IChannel _channel;
    private readonly IConnection _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationEventConsumerBackgroundService"/>
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public IntegrationEventConsumerBackgroundService(
        ILogger<IntegrationEventConsumerBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var factory = new ConnectionFactory
        {
            Uri = new Uri(MessageBrokerSettings.AmqpLink!)
        };

        _connection = factory.CreateConnection();

        _channel = _connection.CreateChannel();

        _channel.QueueDeclare(MessageBrokerSettings.QueueName, false, false, false);

        try
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += OnIntegrationEventReceived!;

            _channel.BasicConsumeAsync(MessageBrokerSettings.QueueName, false, consumer);
        }
        catch (Exception e)
        {
            logger.LogCritical($"ERROR: Failed to process the integration events: {e.Message}", e.Message);
        }
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _channel.CloseAsync();

        _connection.CloseAsync();
    }

    /// <summary>
    /// Processes the integration event received from the message queue.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The event arguments.</param>
    /// <returns>The completed task.</returns>
    private void OnIntegrationEventReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        string body = Encoding.UTF8.GetString(eventArgs.Body.Span);

        var integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(body, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });

        using IServiceScope scope = _serviceProvider.CreateScope();

        var integrationEventConsumer = scope.ServiceProvider.GetRequiredService<IIntegrationEventConsumer>();

        integrationEventConsumer.Consume(integrationEvent);

        _channel.BasicAck(eventArgs.DeliveryTag, false);
    }
}