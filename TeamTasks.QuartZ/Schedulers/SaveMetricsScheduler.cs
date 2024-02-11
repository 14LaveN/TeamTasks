using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using TeamTasks.QuartZ.Jobs;

namespace TeamTasks.QuartZ.Schedulers;

public sealed class SaveMetricsScheduler
{
    public static async void Start()
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            //scheduler.JobFactory = serviceProvider.GetService<QuartzJobFactory>();
        await scheduler.Start();

        IJobDetail jobDetail = JobBuilder.Create<SaveMetricsJob>().Build();
        ITrigger trigger = TriggerBuilder
            .Create()
            .WithIdentity($"{nameof(SaveMetricsJob)}Trigger", "default")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(300)
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}