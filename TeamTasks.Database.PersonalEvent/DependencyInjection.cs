using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamTasks.Database.Common;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.PersonalEvent.Data.Interfaces;
using TeamTasks.Database.PersonalEvent.Data.Repositories;

namespace TeamTasks.Database.PersonalEvent;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddPersonalEventDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        var connectionString = configuration.GetConnectionString("PAGenericDb");
        
        services.AddDbContext<PersonalEventDbContext>(o => 
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
        
        services.AddScoped<IPersonalEventRepository, PersonalEventRepository>();
        services.AddScoped<BaseDbContext<Domain.Entities.PersonalEvent>, PersonalEventDbContext>();
        services.AddScoped<IUnitOfWork<Domain.Entities.PersonalEvent>, UnitOfWork<Domain.Entities.PersonalEvent>>();

        return services;
    }
}