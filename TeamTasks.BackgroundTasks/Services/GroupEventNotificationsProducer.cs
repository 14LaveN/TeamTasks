using TeamTasks.Application.Core.Abstractions.Common;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.GroupEvent.Data.Interfaces;
using TeamTasks.Database.Notification.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;

namespace TeamTasks.BackgroundTasks.Services;

/// <summary>
/// Represents the group event notifications producer.
/// </summary>
internal sealed class GroupEventNotificationsProducer : IGroupEventNotificationsProducer
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IDateTime _dateTime;
    private readonly IUnitOfWork<Notification> _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEventNotificationsProducer"/> class.
    /// </summary>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="notificationRepository">The notification repository.</param>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public GroupEventNotificationsProducer(
        IGroupEventRepository groupEventRepository,
        INotificationRepository notificationRepository,
        IDateTime dateTime,
        IUnitOfWork<Notification> unitOfWork)
    {
        _groupEventRepository = groupEventRepository;
        _notificationRepository = notificationRepository;
        _dateTime = dateTime;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task ProduceAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<PersonalEvent> unprocessedPersonalEvents = await _groupEventRepository.GetUnprocessedAsync(batchSize);

        if (!unprocessedPersonalEvents.Any())
        {
            return;
        }

        var notifications = new List<Notification>();

        foreach (var personalEvent in unprocessedPersonalEvents)
        {
            Result result = personalEvent.MarkAsProcessed();

            if (result.IsFailure)
            {
                continue;
            }

            List<Notification> notificationsForPersonalEvent = NotificationType
                .List
                .Select(notificationType => notificationType.TryCreateNotification(personalEvent, personalEvent.UserId, _dateTime.UtcNow))
                .Where(maybeNotification => maybeNotification.HasValue)
                .Select(maybeNotification => maybeNotification.Value)
                .ToList();

            notifications.AddRange(notificationsForPersonalEvent);
        }

        await _notificationRepository.InsertRange(notifications);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}