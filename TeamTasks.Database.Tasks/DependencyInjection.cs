using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamTasks.Database.Common;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Tasks.Data.Interfaces;
using TeamTasks.Database.Tasks.Data.Repositories;

namespace TeamTasks.Database.Tasks;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddTasksDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        var connectionString = configuration.GetConnectionString("TTGenericDb");
        
        services.AddDbContext<TasksDbContext>(o => 
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
        
        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<BaseDbContext<Domain.Entities.TaskEntity>, TasksDbContext>();
        services.AddScoped<IUnitOfWork<Domain.Entities.TaskEntity>, UnitOfWork<Domain.Entities.TaskEntity>>();

        return services;
    }
}