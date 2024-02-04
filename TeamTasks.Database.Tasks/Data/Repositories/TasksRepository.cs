using Dapper;
using Microsoft.Data.SqlClient;
using TeamTasks.Database.Common;
using TeamTasks.Database.Tasks.Data.Interfaces;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.DTO.Tasks;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Tasks.Data.Repositories;

/// <summary>
/// Represents the tasks repository.
/// </summary>
internal sealed class TasksRepository : GenericRepository<TaskEntity>, ITasksRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TasksRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public TasksRepository(BaseDbContext<TaskEntity> dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc />
    public async Task<Result<TaskEntity>> UpdateTask(TaskEntity task)
    {
        const string sql = """
                           
                                           UPDATE dbo.tasks
                                           SET ModifiedOnUtc= @ModifiedOnUtc,
                                               Priority = @Priority,
                                               Title = @Title,
                                               Description = @Description
                                           WHERE Id = @Id AND Deleted = 0
                           """;
        
        SqlParameter[] parameters =
        {
            new("@ModifiedOnUtc", DateTime.UtcNow),
            new("@Title", task.Title.Value),
            new("@Id", task.Id),
            new("@Priority", task.Priority),
            new("@Description", task.Description)
        };
        int result = await DbContext.ExecuteSqlAsync(sql, parameters);
        
        return result is not 0 ? task : throw new ArgumentException();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Maybe<TasksDto>>> GetAuthorTasksByIsDone(Guid authorId)
    {
        //TODO Write the database path.
        await using var connection = new SqlConnection("");
        
        await connection.OpenAsync();
        
        var query = """
                        SELECT t.Description, t.IsDone, t.Title,
                               t.CreatedAt, t.Priority, u.UserName 
                        FROM dbo.tasks AS t
                        INNER JOIN dbp.AspNetUsers AS u 
                        ON @AuthorId = u.Id
                        GROUP BY t.Description, t.IsDone, t.Title,
                                 t.CreatedAt, t.Priority, u.UserName 
                        
                    """;

        var tasksDto = await connection.QueryAsync<Maybe<TasksDto>>(query, new { AuthorId = authorId });
        
        return tasksDto;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Maybe<TasksDto>>> GetCompanyTasksByIsDone(Guid companyId)
    {
        //TODO Write the database path.
        await using var connection = new SqlConnection("");
        
        await connection.OpenAsync();
        
        var query = $"""
                         SELECT t.Description, t.IsDone, t.Title,
                                t.CreatedAt, t.Priority, c.Id
                         FROM dbo.tasks AS t
                         INNER JOIN dbo.companies AS c
                         ON @ComapnyId = c.Id
                         GROUP BY t.Description, t.IsDone, t.Title,
                                  t.CreatedAt, t.Priority, c.UserName
                         
                     """;

        var tasksDto = await connection.QueryAsync<Maybe<TasksDto>>(query, new { ComapnyId = companyId });
        
        return tasksDto;
    }
}