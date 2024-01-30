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
    }
}