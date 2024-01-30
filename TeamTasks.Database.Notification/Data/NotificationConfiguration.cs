using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamTasks.Domain.Enumerations;
using Event = TeamTasks.Domain.Entities.Event;
using User = TeamTasks.Domain.Entities.User;

namespace TeamTasks.Database.Notification.Data;

/// <summary>
/// Represents the configuration for the <see cref="Notification"/> entity.
/// </summary>
internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Domain.Entities.Notification>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Entities.Notification> builder)
    {
        builder.HasKey(notification => notification.Id);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(notification => notification.EventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(notification => notification.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(notification => notification.NotificationType)
            .HasConversion(p => p.Value, v => NotificationType.FromValue(v))
            .IsRequired();

        builder
            .Property(notification => notification.Sent)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(notification => notification.DateTimeUtc)
            .IsRequired();

        builder
            .Property(invitation => invitation.CreatedOnUtc)
            .IsRequired();

        builder
            .Property(invitation => invitation.ModifiedOnUtc);

        builder
            .Property(invitation => invitation.DeletedOnUtc);

        builder
            .Property(invitation => invitation.Deleted)
            .HasDefaultValue(false);

        builder
            .HasQueryFilter(invitation => !invitation.Deleted);
    }
}