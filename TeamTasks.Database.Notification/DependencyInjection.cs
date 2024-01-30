using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamTasks.Database.Common;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Notification.Data.Interfaces;
using TeamTasks.Database.Notification.Data.Repositories;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Notification;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddNotificationsDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        var connectionString = configuration.GetConnectionString("PAGenericDb");
        
        services.AddDbContext<NotificationDbContext>(o => 
            o.UseNpgsql(connectionString, act 
                    =>
                {
                    act.EnableRetryOnFailure(3);
                    act.CommandTimeout(30);
                })
                .LogTo(Console.WriteLine)
                .EnableServiceProviderCaching()
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

        if (connectionString is not null)
            services.AddHealthChecks()
                .AddNpgSql(connectionString);

        services.AddTransient<BaseDbContext<Event>>();
        
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<BaseDbContext<Domain.Entities.Notification>, NotificationDbContext>();
        services.AddScoped<IUnitOfWork<Domain.Entities.Notification>, UnitOfWork<Domain.Entities.Notification>>();

        return services;
    }
}