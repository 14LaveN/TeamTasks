using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;

namespace TeamTasks.Micro.TasksAPI.Contracts.Task.Create;

/// <summary>
/// Represents the response after create task class.
/// </summary>
/// <param name="Description">The description.</param>
/// <param name="Priority">The task priority.</param>
/// <param name="Title">The title.</param>
/// <param name="AuthorName">The author name.</param>
/// <param name="CreatedAt">The created at date/time.</param>
public sealed record CreateTaskResponse(string Title,
    string AuthorName,
    TaskPriority Priority,
    string Description,
    DateTime CreatedAt)
{
    /// <summary>
    /// Create the <see cref="CreateTaskResponse"/> record from <see cref="TaskEntity"/> class.
    /// </summary>
    /// <param name="taskEntity">The task entity class.</param>
    /// <returns>The new instance of <see cref="CreateTaskResponse"/> record class.</returns>
    public static implicit operator CreateTaskResponse(TaskEntity taskEntity)
    {
        return new CreateTaskResponse(
            taskEntity.Title,
            taskEntity.Author!.UserName!,
            taskEntity.Priority,
            taskEntity.Description,
            taskEntity.CreatedAt);
    }
}