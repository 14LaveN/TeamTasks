using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamTasks.Database.Common;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Tasks;

/// <summary>
/// Represents the application database context task class.
/// </summary>
public class TasksDbContext
    : BaseDbContext<TaskEntity>
{
    /// <summary>
    /// Gets or sets personal events
    /// </summary>
    public DbSet<TaskEntity> Tasks { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="TasksDbContext"/> class.
    /// </summary>
    /// <param name="dbContextOptions">The database context options.</param>
    /// <param name="mediator">The mediator.</param>
    public TasksDbContext(
        DbContextOptions<TasksDbContext> dbContextOptions,
        IMediator mediator)
        : base(dbContextOptions, mediator) {}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=TTGenericDb;User Id=postgres;Password=1111;");
    }
    
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasDefaultSchema("dbo");
        
        base.OnModelCreating(modelBuilder);
    }
}