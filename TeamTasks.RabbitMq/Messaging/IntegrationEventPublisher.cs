﻿using System.Text;
using System.Text.Json;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.RabbitMq.Messaging.Settings;
using RabbitMQ.Client;

namespace TeamTasks.RabbitMq.Messaging;

/// <summary>
/// Represents the integration event publisher.
/// </summary>
public sealed class IntegrationEventPublisher : IIntegrationEventPublisher
{
    /// <summary>
    /// Initialize connection.
    /// </summary>
    /// <returns>Returns connection to RabbitMQ.</returns> 
    private static async Task<IConnection> CreateConnection()
    {
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(MessageBrokerSettings.AmqpLink!)
        };

        var connection = await connectionFactory.CreateConnectionAsync();

        return connection;
    }
    
    /// <summary>
    /// Initialize channel.
    /// </summary>
    /// <returns>Returns channel for RabbitMQ.</returns>
    private static async Task<IChannel> CreateChannel()
    {
        var connection = await CreateConnection();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(MessageBrokerSettings.QueueName, false, false, false);
        await channel.QueueBindAsync(MessageBrokerSettings.QueueName,
            exchange: MessageBrokerSettings.QueueName + "Exchange",
            routingKey: MessageBrokerSettings.QueueName);

        return channel;
    }

    /// <inheritdoc />
    public async Task Publish(IIntegrationEvent integrationEvent)
    {
        var channel = await CreateChannel();
        string payload = JsonSerializer.Serialize(integrationEvent, typeof(IIntegrationEvent));

        var body = Encoding.UTF8.GetBytes(payload);

        if (MessageBrokerSettings.QueueName is not null)
            await channel.BasicPublishAsync(MessageBrokerSettings.QueueName + "Exchange",
                MessageBrokerSettings.QueueName, body: body);

        await channel.CloseAsync();
    }
}