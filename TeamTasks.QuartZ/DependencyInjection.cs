using Microsoft.Extensions.DependencyInjection;
using TeamTasks.QuartZ.Jobs;
using Quartz.Impl;
using Quartz.Spi;
using TeamTasks.QuartZ.Schedulers;

namespace TeamTasks.QuartZ;

public static class DependencyInjection
{
    public static IServiceCollection AddQuartzExtensions(this IServiceCollection services)
    {
        services.AddTransient<IJobFactory, QuartzJobFactory>();
        services.AddSingleton(provider =>
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            return scheduler;
        });
        services.AddTransient<AbstractScheduler<UserDbTask>>();
        services.AddTransient<SaveMetricsScheduler>();
        
        return services;
    }
}