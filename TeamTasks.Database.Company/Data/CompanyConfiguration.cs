using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Company.Data;

/// <summary>
/// Represents the configuration for the <see cref="TaskEntity"/> class.
/// </summary>
internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Domain.Entities.Company>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Entities.Company> builder)
    {
        builder.ToTable("companies");

        builder.HasIndex(x => x.Id)
            .HasDatabaseName("IdCompanyIndex")
            .IsUnique();
        
        builder.HasIndex(x => x.CompanyName)
            .HasDatabaseName("NameCompanyIndex")
            .IsUnique();

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.HasMany(x => x.Tasks)
            .WithOne(x => x.Company)
            .HasForeignKey(x=>x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Author)
            .WithOne(x => x.Company)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Users)
            .WithOne(x => x.Company)
            .HasForeignKey(x=>x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}