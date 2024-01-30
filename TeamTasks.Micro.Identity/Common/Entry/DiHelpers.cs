using TeamTasks.Application.Core.Helpers.Metric;

namespace TeamTasks.Micro.Identity.Common.Entry;

public static class DiHelpers
{
    public static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<CreateMetricsHelper>();
        
        return services;
    }
}