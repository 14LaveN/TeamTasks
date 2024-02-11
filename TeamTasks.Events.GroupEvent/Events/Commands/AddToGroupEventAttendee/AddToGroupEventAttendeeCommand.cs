using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Events.GroupEvent.Events.Commands.AddToGroupEventAttendee;

//TODO Create the handlers and think about logic group events.

/// <summary>
/// Represents the add to group event attendee command record class.
/// </summary>
/// <param name="GroupEventId">The group event identifier.</param>
/// <param name="Attendee">The attendee.</param>
public sealed record AddToGroupEventAttendeeCommand(
    Guid GroupEventId,
    User Attendee) : ICommand<IBaseResponse<Result>>;