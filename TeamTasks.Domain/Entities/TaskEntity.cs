using TeamTasks.Domain.Core.Abstractions;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Core.Utility;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Domain.Events;
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
    /// <param name="companyId">The company identifier.</param>
    public TaskEntity(Name title, Guid authorId, TaskPriority priority, DateTime createdAt, bool isDone, string description, Guid companyId)
        : base(Guid.NewGuid())
    {
        Ensure.NotNull(title, "The name is required.", nameof(title));
        Ensure.NotEmpty(authorId, "The author identifier is required", nameof(authorId));
        Ensure.NotEmpty(createdAt, "The date and time is required.", nameof(createdAt));
        Ensure.NotNull(description, "The description is required.", nameof(description));
        Ensure.NotEmpty(companyId, "The company identifier is required", nameof(companyId));
        
        IsDone = isDone;
        Title = title;
        AuthorId = authorId;
        Description = description;
        CompanyId = companyId;
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
    /// Gets or sets company identifier.
    /// </summary>
    public Guid CompanyId { get; }

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

    /// <summary>
    /// Creates a new task with the specified author id, createdAt, title and description.
    /// </summary>
    /// <param name="authorId">The author id.</param>
    /// <param name="createdAt">The created at.</param>
    /// <param name="priority">The task priority.</param>
    /// <param name="description">The description.</param>
    /// <param name="title">The title.</param>
    /// <returns>The newly created answer instance.</returns>
    public static TaskEntity Create(
        Guid authorId, 
        TaskPriority priority,
        string description,
        Name title)
    {
        var task = new TaskEntity(
            title,
            authorId,
            priority,
            DateTime.UtcNow, 
            false,
            description,
            default);

        task.AddDomainEvent(new TaskCreatedDomainEvent(task));
        return task;
    }
    
    public async Task<Result> DoneTask(TaskEntity answerEntity, User user)
    {
        if (IsDone)
        {
            return Result.Failure(DomainErrors.Notification.AlreadySent);
        }

        IsDone = true;

        AddDomainEvent(new DoneTaskDomainEvent(answerEntity, user));

        return await Result.Success();
    }
}