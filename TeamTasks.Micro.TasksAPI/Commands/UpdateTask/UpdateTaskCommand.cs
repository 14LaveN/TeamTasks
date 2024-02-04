using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Domain.ValueObjects;

namespace TeamTasks.Micro.TasksAPI.Commands.UpdateTask;

/// <summary>
///  Represents the <see cref="UpdateTaskCommand"/> record class.
/// </summary>
///<param name="Title">The title.</param>
/// <param name="Priority">The task priority.</param>
/// <param name="Description">The description.</param>
/// <param name="TaskId">The task identifier.</param>
public sealed record UpdateTaskCommand(
        Guid TaskId,
        Name Title,
        TaskPriority Priority,
        string Description)
    : ICommand<IBaseResponse<Result>>;