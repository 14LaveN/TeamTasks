using TeamTasks.Domain.Core.Abstractions;
using TeamTasks.Domain.Core.Primitives;
using TeamTasks.Domain.Core.Utility;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Domain.ValueObjects;

namespace TeamTasks.Domain.Entities;

/// <summary>
/// Represents the task entity class.
/// </summary>
public sealed class TaskEntity
    : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskEntity"/> class.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="authorId">The task author identifier.</param>
    /// <param name="priority">The task priority.</param>
    /// <param name="createdAt">The created at date/time.</param>
    /// <param name="isDone">The is done checkbox.</param>
    /// <param name="description">The description.</param>
    public TaskEntity(Name title, Guid authorId, TaskPriority priority, DateTime createdAt, bool isDone, string description)
        : base(Guid.NewGuid())
    {
        Ensure.NotNull(title, "The name is required.", nameof(title));
        Ensure.NotEmpty(authorId, "The author identifier is required", nameof(authorId));
        Ensure.NotEmpty(createdAt, "The date and time is required.", nameof(createdAt));
        Ensure.NotNull(description, "The description is required.", nameof(description));
        
        IsDone = isDone;
        Title = title;
        AuthorId = authorId;
        Description = description;
        Priority = priority;
        CreatedAt = createdAt;
    }
    
    /// <summary>
    /// Gets or sets title.
    /// </summary>
    public Name Title { get; set; }
    
    /// <summary>
    /// Gets or sets author class.
    /// </summary>
    public User? Author { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Company"/> class.
    /// </summary>
    public Company? Company { get; set; }

    /// <summary>
    /// Gets or sets author identifier.
    /// </summary>
    public Guid AuthorId { get; }
    
    /// <summary>
    /// Gets or sets is done checkbox.
    /// </summary>
    public bool IsDone { get; set; }
    
    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets or sets date/time created at.
    /// </summary>
    public DateTime CreatedAt { get; }
    
    /// <summary>
    /// Gets or sets task priority.
    /// </summary>
    public TaskPriority Priority { get; }

    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; }
    
    /// <inheritdoc />
    public DateTime? ModifiedOnUtc { get; }
    
    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; }
    
    /// <inheritdoc />
    public bool Deleted { get; }
}