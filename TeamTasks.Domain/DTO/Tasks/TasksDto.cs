using TeamTasks.Domain.Enumerations;

namespace TeamTasks.Domain.DTO.Tasks;

/// <summary>
/// Represents the tasks dto record.
/// </summary>
public sealed record TasksDto(string AuthorName,
    string Description,
    bool IsDone,
    string Title,
    DateTime CreatedAt,
    TaskPriority TaskPriority);