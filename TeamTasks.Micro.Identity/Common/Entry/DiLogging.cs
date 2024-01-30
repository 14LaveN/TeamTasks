using NLog.Web;

namespace TeamTasks.Micro.Identity.Common.Entry;

public static class DiLogging
{
    public static IServiceCollection AddLoggingExtension(this IServiceCollection services, ConfigureHostBuilder host)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            loggingBuilder.AddNLogWeb("nlogIdentity.config");
        });
        host.UseNLog();
        
        return services;
    }
}