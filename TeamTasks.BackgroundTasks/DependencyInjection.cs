using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamTasks.BackgroundTasks.Services;
using TeamTasks.BackgroundTasks.Settings;
using TeamTasks.BackgroundTasks.Tasks;

namespace TeamTasks.BackgroundTasks;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddBackgroundTasks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(x=>
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.Configure<BackgroundTaskSettings>(configuration.GetSection(BackgroundTaskSettings.SettingsKey));

        services.AddHostedService<GroupEventNotificationsProducerBackgroundService>();

        services.AddHostedService<PersonalEventNotificationsProducerBackgroundService>();

        services.AddHostedService<EmailNotificationConsumerBackgroundService>();

        services.AddHostedService<IntegrationEventConsumerBackgroundService>();


        services.AddScoped<IPersonalEventNotificationsProducer, PersonalEventNotificationsProducer>();

        services.AddScoped<IEmailNotificationsConsumer, EmailNotificationsConsumer>();

        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

        return services;
    }
}