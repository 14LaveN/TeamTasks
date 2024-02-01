using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Domain.ValueObjects;

namespace TeamTasks.Micro.TasksAPI.Commands.CreateTask;

/// <summary>
/// Represents the <see cref="CreateTaskCommand"/> record class.
/// </summary>
/// <param name="Title">The title.</param>
/// <param name="AuthorId">The author identifier.</param>
/// <param name="CompanyId">The company identifier.</param>
/// <param name="Priority">The task priority.</param>
/// <param name="Description">The description.</param>
public sealed record CreateTaskCommand(
    Name Title,
    Guid AuthorId,
    TaskPriority Priority,
    string Description,
    Guid CompanyId)
    : ICommand<IBaseResponse<Result>>
{
    /// <summary>
    /// Create the answer entity class from <see cref="CreateTaskCommand"/> record.
    /// </summary>
    /// <param name="command">The create task command.</param>
    /// <returns>The new task entity.</returns>
    public static implicit operator TaskEntity(CreateTaskCommand command)
    {
        return new TaskEntity(
            command.Title,
            command.AuthorId,
            command.Priority,
            DateTime.UtcNow,
            false,
            command.Description,
            command.CompanyId);
    }
}