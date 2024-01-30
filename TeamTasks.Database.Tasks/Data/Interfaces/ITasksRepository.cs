using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.DTO.Tasks;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Tasks.Data.Interfaces;

/// <summary>
/// Represents the tasks repository interface.
/// </summary>
public interface ITasksRepository
{
    /// <summary>
    /// Gets the task with the specified identifier.
    /// </summary>
    /// <param name="taskId">The task identifier.</param>
    /// <returns>The maybe instance that may contain the task entity with the specified identifier.</returns>
    Task<Maybe<TaskEntity>> GetByIdAsync(Guid taskId);

    /// <summary>
    /// Inserts the specified task entity to the database.
    /// </summary>
    /// <param name="task">The task to be inserted to the database.</param>
    Task Insert(TaskEntity task);

    /// <summary>
    /// Remove the specified task entity to the database.
    /// </summary>
    /// <param name="task">The task to be inserted to the database.</param>
    void Remove(TaskEntity task);

    /// <summary>
    /// Update the specified task entity to the database.
    /// </summary>
    /// <param name="task">The task to be inserted to the database.</param>
    /// <returns>The result instance that may contain the task entity with the specified task class.</returns>
    Task<Result<TaskEntity>> UpdateTask(TaskEntity task);

    /// <summary>
    /// Gets the enumerable tasks with the specified author identifier.
    /// </summary>
    /// <param name="authorId">The author identifier.</param>
    /// <returns>The maybe instance that may contain the enumerable task DTO with the specified task class.</returns>
    Task<IEnumerable<Maybe<TasksDto>>> GetAuthorTasksByIsDone(Guid authorId);
    
    /// <summary>
    /// Gets the enumerable tasks with the specified company identifier.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <returns>The maybe instance that may contain the enumerable task DTO with the specified task class.</returns>
    Task<IEnumerable<Maybe<TasksDto>>> GetCompanyTasksByIsDone(Guid companyId);
}