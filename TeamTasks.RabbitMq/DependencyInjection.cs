using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamTasks.RabbitMq.Messaging;
using TeamTasks.RabbitMq.Messaging.Settings;

namespace TeamTasks.RabbitMq;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingsKey));
        
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services.AddHealthChecks()
            .AddRabbitMQ();
        
        return services; 
    }
}