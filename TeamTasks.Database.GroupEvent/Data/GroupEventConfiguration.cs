using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamTasks.Database.GroupEvent.Data;

public class GroupEventConfiguration : IEntityTypeConfiguration<Domain.Entities.GroupEvent>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Entities.GroupEvent> builder) =>
        builder.ToTable("groupEvents");
}