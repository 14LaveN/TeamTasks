using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Primitives.Result;

namespace TeamTasks.Events.GroupEvent.Events.Commands.CreateGroupEvent;

/// <summary>
/// Represents the create group event command.
/// </summary>
public sealed record CreateGroupEventCommand(
    Guid UserId,
    string Name, 
    int CategoryId, 
    DateTime DateTimeUtc) : ICommand<Result>;