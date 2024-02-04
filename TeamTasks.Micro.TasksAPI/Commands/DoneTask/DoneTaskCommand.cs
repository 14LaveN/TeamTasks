using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Primitives.Result;

namespace TeamTasks.Micro.TasksAPI.Commands.DoneTask;

/// <summary>
/// Represents the <see cref="DoneTaskCommand"/> record class.
/// </summary>
/// <param name="TaskId">The task identifier.</param>
public sealed record DoneTaskCommand(Guid TaskId)
    : ICommand<IBaseResponse<Result>>
{
    /// <summary>
    /// Create a new instance of the <see cref="DoneTaskCommand"/> record class from task identifier.
    /// </summary>
    /// <param name="taskId">The task identifier.</param>
    /// <returns>The new <see cref="DoneTaskCommand"/> record class.</returns>
    public static implicit operator DoneTaskCommand(Guid taskId)
    {
        return new DoneTaskCommand(taskId);
    }
}