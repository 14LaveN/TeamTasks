using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.ValueObjects;

namespace TeamTasks.Database.Identity.Data;

/// <summary>
/// Represents the configuration for the <see cref="User"/> entity.
/// </summary>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.HasIndex(x => x.Id)
            .HasDatabaseName("IdUserIndex");

        builder.HasData(User.Create(FirstName.Create("dfsdf").Value,
            LastName.Create("fdfsdfsf").Value,
            new EmailAddress("dfsdfsdfdsf"),
            "Sdfdsf_2008",
            Guid.Empty));
        
        builder.HasKey(user => user.Id);

        builder.HasOne(x => x.Company)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(user => user.FirstName, firstNameBuilder =>
        {
            firstNameBuilder.WithOwner();

            firstNameBuilder.Property(firstName => firstName.Value)
                .HasColumnName(nameof(User.FirstName))
                .HasMaxLength(FirstName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(user => user.LastName, lastNameBuilder =>
        {
            lastNameBuilder.WithOwner();

            lastNameBuilder.Property(lastName => lastName.Value)
                .HasColumnName(nameof(User.LastName))
                .HasMaxLength(LastName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(user => user.EmailAddress, emailBuilder =>
        {
            emailBuilder.WithOwner();

            emailBuilder.Property(email => email.Value)
                .HasColumnName(nameof(User.EmailAddress))
                .HasMaxLength(EmailAddress.MaxLength)
                .IsRequired();
        });

        builder.Property<string>("_passwordHash")
            .HasField("_passwordHash")
            .HasColumnName("PasswordHash")
            .IsRequired();

        builder.Property(x => x.CompanyId)
            .HasField("CompanyId")
            .HasColumnName("CompanyId")
            .HasDefaultValue(Guid.Empty);

        builder.Property(user => user.CreatedOnUtc).IsRequired();

        builder.Property(user => user.ModifiedOnUtc);

        builder.Property(user => user.DeletedOnUtc);

        builder.Property(user => user.Deleted).HasDefaultValue(false);

        builder.HasQueryFilter(user => !user.Deleted);

        builder.Ignore(user => user.FullName);
    }
}