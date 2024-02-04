using TeamTasks.Database.Identity;
using TeamTasks.Database.MetricsAndMessages;
using TeamTasks.Database.Tasks;

namespace TeamTasks.Micro.TasksAPI.Common.DependencyInjection;

public static class DiDatabase
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddUserDatabase(configuration);
        services.AddMongoDatabase(configuration);
        services.AddTasksDatabase(configuration);
        
        return services;
    }
}