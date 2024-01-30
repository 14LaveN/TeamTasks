using TeamTasks.Email.Contracts.Emails;

namespace TeamTasks.Application.Core.Abstractions.Notifications;

/// <summary>
/// Represents the emailAddress notification service interface.
/// </summary>
public interface IEmailNotificationService
{
    /// <summary>
    /// Sends the welcome emailAddress notification based on the specified request.
    /// </summary>
    /// <param name="welcomeEmail">The welcome emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendWelcomeEmail(WelcomeEmail welcomeEmail);

    /// <summary>
    /// Sends the attendee created emailAddress.
    /// </summary>
    /// <param name="attendeeCreatedEmail">The attendee created emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendAttendeeCreatedEmail(AttendeeCreatedEmail attendeeCreatedEmail);

    /// <summary>
    /// Sends the group event cancelled emailAddress.
    /// </summary>
    /// <param name="groupEventCancelledEmail">The group event cancelled emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendGroupEventCancelledEmail(GroupEventCancelledEmail groupEventCancelledEmail);

    /// <summary>
    /// Sends the group event name changed emailAddress.
    /// </summary>
    /// <param name="groupEventNameChangedEmail">The group event name changed emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendGroupEventNameChangedEmail(GroupEventNameChangedEmail groupEventNameChangedEmail);

    /// <summary>
    /// Sends the group event date and time changed emailAddress.
    /// </summary>
    /// <param name="groupEventDateAndTimeChangedEmail">The group event date and time changed emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendGroupEventDateAndTimeChangedEmail(GroupEventDateAndTimeChangedEmail groupEventDateAndTimeChangedEmail);

    /// <summary>
    /// Sends the invitation sent emailAddress.
    /// </summary>
    /// <param name="invitationSentEmail">The invitation sent emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendInvitationSentEmail(InvitationSentEmail invitationSentEmail);

    /// <summary>
    /// Sends the invitation accepted emailAddress.
    /// </summary>
    /// <param name="invitationAcceptedEmail">The invitation accepted emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendInvitationAcceptedEmail(InvitationAcceptedEmail invitationAcceptedEmail);

    /// <summary>
    /// Sends the invitation rejected emailAddress.
    /// </summary>
    /// <param name="invitationRejectedEmail">The invitation rejected emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendInvitationRejectedEmail(InvitationRejectedEmail invitationRejectedEmail);

    /// <summary>
    /// Sends the password changed emailAddress.
    /// </summary>
    /// <param name="passwordChangedEmail">The password changed emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendPasswordChangedEmail(PasswordChangedEmail passwordChangedEmail);

    /// <summary>
    /// Sends the notification emailAddress.
    /// </summary>
    /// <param name="notificationEmail">The notification emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendNotificationEmail(NotificationEmail notificationEmail);
}