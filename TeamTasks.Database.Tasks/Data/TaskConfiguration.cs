using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Tasks.Data;

/// <summary>
/// Represents the configuration for the <see cref="TaskEntity"/> class.
/// </summary>
internal sealed class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("tasks");

        #region Fields
        builder.HasIndex(x => x.Id)
            .HasDatabaseName("IdTaskIndex")
            .IsUnique();

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.CompanyId)
            .HasField("CompanyId")
            .HasColumnName("CompanyId")
            .HasDefaultValue(Guid.Empty);
        
        builder.Property(x => x.PerformerOfWorkId)
            .HasField("PerformerOfWorkId")
            .HasColumnName("PerformerOfWorkId")
            .HasDefaultValue(Guid.Empty);
        #endregion
        
        #region RelationShip
        builder.HasOne(x => x.Company)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x=>x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Author)
            .WithMany(x => x.YourTasks)
            .HasForeignKey(x=>x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.PerformerOfWork)
            .WithMany(x => x.CompletedTasks)
            .HasForeignKey(x=>x.PerformerOfWorkId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion
    }
}