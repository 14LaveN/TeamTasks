using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamTasks.Database.Common;

namespace TeamTasks.Database.Company;

/// <summary>
/// Represents the application database context company class.
/// </summary>
public class CompanyDbContext
    : BaseDbContext<Domain.Entities.Company>
{
    /// <summary>
    /// Gets or sets personal events
    /// </summary>
    public DbSet<Domain.Entities.Company> Companies { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyDbContext"/> class.
    /// </summary>
    /// <param name="dbContextOptions">The database context options.</param>
    /// <param name="mediator">The mediator.</param>
    public CompanyDbContext(
        DbContextOptions<CompanyDbContext> dbContextOptions,
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