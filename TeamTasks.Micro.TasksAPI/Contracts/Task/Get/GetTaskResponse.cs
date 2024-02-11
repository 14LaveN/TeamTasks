using TeamTasks.Domain.DTO.Tasks;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;

namespace TeamTasks.Micro.TasksAPI.Contracts.Task.Get;

/// <summary>
/// Represents the response after create task class.
/// </summary>
/// <param name="Description">The description.</param>
/// <param name="Priority">The task priority.</param>
/// <param name="Title">The title.</param>
/// <param name="AuthorName">The author name.</param>
/// <param name="CreatedAt">The created at date/time.</param>
public sealed record GetTaskResponse(string Title,
    string AuthorName,
    string Priority,
    string Description,
    string? CompanyName,
    DateTime CreatedAt)
{
    /// <summary>
    /// Create the <see cref="GetTaskResponse"/> record from <see cref="TaskEntity"/> class.
    /// </summary>
    /// <param name="taskEntity">The task entity class.</param>
    /// <returns>The new instance of <see cref="GetTaskResponse"/> record class.</returns>
    public static implicit operator GetTaskResponse(TaskEntity taskEntity)
    {
        return new GetTaskResponse(
            taskEntity.Title,
            taskEntity.Author!.UserName!,
            taskEntity.Priority.ToString(),
            taskEntity.Description,
            default,
            taskEntity.CreatedAt);
    }

    public static IEnumerable<GetTaskResponse> ToGetTaskResponse(IEnumerable<TasksDto> dtos)
    {
        var enumerable = new List<GetTaskResponse>();
        
        foreach (var index in dtos)
        {
            enumerable.Add(new GetTaskResponse(
                index.Title,
                index.AuthorName,
                index.TaskPriority.ToString(),
                index.Description,
                default,
                index.CreatedAt));
        }

        return enumerable;
    }
}