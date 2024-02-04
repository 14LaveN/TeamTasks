using TeamTasks.Application.Core.Abstractions.Notifications;
using TeamTasks.BackgroundTasks.Abstractions.Messaging;
using TeamTasks.BackgroundTasks.IntegrationEvents.Users.UserCreated;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Database.Tasks.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Exceptions;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;
using TeamTasks.Email.Contracts.Emails;
using TeamTasks.RabbitMq.Messaging.Tasks.DoneTask;

namespace TeamTasks.BackgroundTasks.IntegrationEvents.Tasks.TaskIsDone;

/// <summary>
/// Represents the <see cref="DoneTaskIntegrationEvent"/> handler.
/// </summary>
public sealed class SendCompanyEmailOnDoneTaskIntegrationEventHandler
    : IIntegrationEventHandler<DoneTaskIntegrationEvent>
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IEmailNotificationService _emailNotificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendCompanyEmailOnDoneTaskIntegrationEventHandler"/> class.
    /// </summary>
    /// <param name="tasksRepository">The tasks repository.</param>
    /// <param name="emailNotificationService">The emailAddress notification service.</param>
    public SendCompanyEmailOnDoneTaskIntegrationEventHandler(
        ITasksRepository tasksRepository,
        IEmailNotificationService emailNotificationService)
    {
        _emailNotificationService = emailNotificationService;
        _tasksRepository = tasksRepository;
    }
    
    /// <inheritdoc />
    public async Task Handle(
        DoneTaskIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        Maybe<TaskEntity> maybeTask = await _tasksRepository.GetByIdAsync(notification.TaskId);

        if (maybeTask.HasNoValue)
        {
            throw new DomainException(DomainErrors.Task.NotFound);
        }

        TaskEntity task = maybeTask.Value;

        var doneTaskEmail = new DoneTaskEmail(task.Author!.EmailAddress, task.Author.FullName);

        await _emailNotificationService.SendDoneTaskEmail(doneTaskEmail);
    }
}