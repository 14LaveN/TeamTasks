using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Primitives.Result;

namespace TeamTasks.Events.GroupEvent.Events.Commands.CancelGroupEvent;

/// <summary>
/// Represents the cancel group event command.
/// </summary>
public sealed record CancelGroupEventCommand(
        Guid GroupEventId,
        Guid UserId) : ICommand<Result>;