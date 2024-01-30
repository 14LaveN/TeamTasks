using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamTasks.Database.PersonalEvent.Data;

/// <summary>
/// Represents the configuration for the <see cref="PersonalEvent"/> entity.
/// </summary>
internal sealed class PersonalEventConfiguration : IEntityTypeConfiguration<Domain.Entities.PersonalEvent>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Entities.PersonalEvent> builder)
    {
        builder.ToTable("personalEvents");
        
        builder.Property(personalEvent => personalEvent.Processed)
            .IsRequired()
            .HasDefaultValue(false);
    }
}