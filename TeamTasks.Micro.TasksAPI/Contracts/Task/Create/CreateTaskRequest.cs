using TeamTasks.Domain.Enumerations;

namespace TeamTasks.Micro.TasksAPI.Contracts.Task.Create;

public record CreateTaskRequest(string Title,
    TaskPriority Priority,
    string Description);