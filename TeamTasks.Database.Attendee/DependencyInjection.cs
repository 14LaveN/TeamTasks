using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamTasks.Database.Attendee.Data.Interfaces;
using TeamTasks.Database.Attendee.Data.Repositories;
using TeamTasks.Database.Common;
using TeamTasks.Database.Common.Abstractions;

namespace TeamTasks.Database.Attendee;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddAttendeesDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        var connectionString = configuration.GetConnectionString("PAGenericDb");
        
        services.AddDbContext<AttendeeDbContext>(o => 
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

        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<BaseDbContext<Domain.Entities.Attendee>, AttendeeDbContext>();
        services.AddScoped<IUnitOfWork<Domain.Entities.Attendee>, UnitOfWork<Domain.Entities.Attendee>>();

        return services;
    }
}