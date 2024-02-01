using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamTasks.Database.Common;

namespace TeamTasks.Database.PersonalEvent;

/// <summary>
/// Represents the application database context group event class.
/// </summary>
public class PersonalEventDbContext
    : BaseDbContext<Domain.Entities.PersonalEvent>
{
    /// <summary>
    /// Gets or sets personal events
    /// </summary>
    public DbSet<Domain.Entities.PersonalEvent> PersonalEvents { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalEventDbContext"/> class.
    /// </summary>
    /// <param name="dbContextOptions">The database context options.</param>
    /// <param name="mediator"></param>
    public PersonalEventDbContext(
        DbContextOptions<PersonalEventDbContext> dbContextOptions,
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