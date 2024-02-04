using MediatR;
using MediatR.NotificationPublishers;
using TeamTasks.Application.Core.Behaviours;
using TeamTasks.Micro.TasksAPI.Behaviors;
using TeamTasks.Micro.TasksAPI.Commands.CreateTask;
using TeamTasks.Micro.TasksAPI.Commands.DoneTask;
using TeamTasks.Micro.TasksAPI.Commands.UpdateTask;

namespace TeamTasks.Micro.TasksAPI.Common.DependencyInjection;

public static class DiMediator
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMediatrExtension(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining<Program>();

            x.RegisterServicesFromAssemblies(typeof(CreateTaskCommand).Assembly,
                typeof(CreateTaskCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(DoneTaskCommand).Assembly,
                typeof(DoneTaskCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(UpdateTaskCommand).Assembly,
                typeof(UpdateTaskCommandHandler).Assembly);
            
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TaskTransactionBehavior<,>));
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));
            x.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
            
            x.NotificationPublisher = new ForeachAwaitPublisher();
        });
        
        return services;
    }
}