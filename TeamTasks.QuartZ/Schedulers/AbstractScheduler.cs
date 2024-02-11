using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using TeamTasks.QuartZ.Jobs;

namespace TeamTasks.QuartZ.Schedulers;

public sealed class AbstractScheduler<T> 
    where T: IJob
{
    public static async void Start(IServiceCollection serviceProvider)
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            //scheduler.JobFactory = serviceProvider.GetService<QuartzJobFactory>();
        await scheduler.Start();

        IJobDetail jobDetail = JobBuilder.Create<UserDbTask>().Build();
        ITrigger trigger = TriggerBuilder
            .Create()
            .WithIdentity($"{nameof(UserDbTask)}Trigger", "default")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(160)
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}