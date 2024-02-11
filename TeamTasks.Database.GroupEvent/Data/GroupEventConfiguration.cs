using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamTasks.Database.GroupEvent.Data;

public class GroupEventConfiguration : IEntityTypeConfiguration<Domain.Entities.GroupEvent>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Entities.GroupEvent> builder)
    {
        builder.ToTable("groupEvents");

        builder.HasMany(x => x.Attendees)
            .WithMany(x => x.AttendeeGroupEvents);
        
        builder.HasOne(x => x.Author)
            .WithMany(x => x.YourGroupEvents)
            .HasForeignKey(x=>x.UserId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}