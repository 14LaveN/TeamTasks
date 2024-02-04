using TeamTasks.Domain.Core.Abstractions;
using TeamTasks.Domain.Core.Primitives;
using TeamTasks.Domain.Core.Utility;
using TeamTasks.Domain.ValueObjects;

namespace TeamTasks.Domain.Entities;

/// <summary>
/// Represents the company entity class.
/// </summary>
public sealed class Company
    : AggregateRoot, ISoftDeletableEntity, IAuditableEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Company"/>class.
    /// </summary>
    /// <param name="companyName">The company name.</param>
    /// <param name="description">The description.</param>
    /// <param name="createdAt">The created at date/time.</param>
    public Company(Name companyName, string description, DateTime createdAt)
        : base(Guid.NewGuid())
    {
        Ensure.NotNull(companyName, "The company name is required.", nameof(companyName));
        Ensure.NotNull(description, "The description is required.", nameof(description));
        Ensure.NotEmpty(createdAt, "The date and time is required.", nameof(createdAt));

        CreatedAt = createdAt;
        CompanyName = companyName;
        Description = description;
    }

    public Company() { }

    /// <summary>
    /// Gets or sets name.
    /// </summary>
    public Name CompanyName { get; set; } = null!;

    /// <summary>
    /// Gets or sets date/time created at.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Navigation field.
    /// </summary>
    public ICollection<TaskEntity>? Tasks { get; set; }

    /// <summary>
    /// Navigation field.
    /// </summary>
    public ICollection<User>? Users { get; set; }
    
    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; }
    
    /// <inheritdoc />
    public DateTime? ModifiedOnUtc { get; }
    
    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; }
    
    /// <inheritdoc />
    public bool Deleted { get; }
}