using System.Globalization;
using TeamTasks.Application.Core.Abstractions.Notifications;
using TeamTasks.BackgroundTasks.Abstractions.Messaging;
using TeamTasks.Database.Attendee.Data.Interfaces;
using TeamTasks.Database.GroupEvent.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Exceptions;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;
using TeamTasks.Email.Contracts.Emails;
using TeamTasks.Events.GroupEvent.Events.Events.GroupEventDateAndTimeChanged;

namespace TeamTasks.BackgroundTasks.IntegrationEvents.GroupEvents.GroupEventDateAndTimeChanged;

/// <summary>
/// Represents the <see cref="GroupEventDateAndTimeChangedIntegrationEvent"/> class.
/// </summary>
internal sealed class NotifyAttendeesOnGroupEventDateAndTimeChangedIntegrationEventHandler
    : IIntegrationEventHandler<GroupEventDateAndTimeChangedIntegrationEvent>
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IEmailNotificationService _emailNotificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotifyAttendeesOnGroupEventDateAndTimeChangedIntegrationEventHandler"/> class.
    /// </summary>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="attendeeRepository">The attendee repository.</param>
    /// <param name="emailNotificationService">The emailAddress notification service.</param>
    public NotifyAttendeesOnGroupEventDateAndTimeChangedIntegrationEventHandler(
        IGroupEventRepository groupEventRepository,
        IAttendeeRepository attendeeRepository,
        IEmailNotificationService emailNotificationService)
    {
        _groupEventRepository = groupEventRepository;
        _attendeeRepository = attendeeRepository;
        _emailNotificationService = emailNotificationService;
    }

    /// <inheritdoc />
    public async Task Handle(GroupEventDateAndTimeChangedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        Maybe<GroupEvent> maybeGroupEvent = await _groupEventRepository.GetByIdAsync(notification.GroupEventId);

        if (maybeGroupEvent.HasNoValue)
        {
            throw new DomainException(DomainErrors.GroupEvent.NotFound);
        }

        GroupEvent groupEvent = maybeGroupEvent.Value;

        (string Email, string Name)[] attendeeEmailsAndNames = await _attendeeRepository.GetEmailsAndNamesForGroupEvent(groupEvent);

        if (attendeeEmailsAndNames.Length == 0)
        {
            return;
        }

        IEnumerable<Task> sendGroupEventCancelledEmailTasks = attendeeEmailsAndNames
            .Select(emailAndName =>
                new GroupEventDateAndTimeChangedEmail(
                    emailAndName.Email,
                    emailAndName.Name,
                    groupEvent.Name,
                    notification.PreviousDateAndTimeUtc.ToString(CultureInfo.InvariantCulture),
                    groupEvent.DateTimeUtc.ToString(CultureInfo.InvariantCulture)))
            .Select(groupEventDateAndTimeChangedEmail =>
                _emailNotificationService.SendGroupEventDateAndTimeChangedEmail(groupEventDateAndTimeChangedEmail));

        await Task.WhenAll(sendGroupEventCancelledEmailTasks);
    }
}