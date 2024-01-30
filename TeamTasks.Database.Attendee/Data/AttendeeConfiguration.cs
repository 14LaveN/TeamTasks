using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Event = TeamTasks.Domain.Entities.Event;
using User = TeamTasks.Domain.Entities.User;

namespace TeamTasks.Database.Attendee.Data;

/// <summary>
/// Represents the configuration for the <see cref="Attendee"/> entity.
/// </summary>
internal sealed class AttendeeConfiguration : IEntityTypeConfiguration<Domain.Entities.Attendee>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Entities.Attendee> builder)
    {
        builder.ToTable("attendees");
        
        builder.HasKey(attendee => attendee.Id);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(attendee => attendee.EventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(attendee => attendee.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(attendee => attendee.Processed).IsRequired();

        builder.Property(attendee => attendee.CreatedOnUtc).IsRequired();

        builder.Property(attendee => attendee.ModifiedOnUtc);

        builder.Property(attendee => attendee.DeletedOnUtc);

        builder.Property(attendee => attendee.Deleted).HasDefaultValue(false);

        builder.HasQueryFilter(attendee => !attendee.Deleted);
    }
}