using MediatR;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Domain.Events;
using TeamTasks.Domain.ValueObjects;

namespace TeamTasks.Domain.Entities;

/// <summary>
/// Represents a group event.
/// </summary>
public sealed class GroupEvent : Event
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEvent"/> class.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="name">The event name.</param>
    /// <param name="category">The category.</param>
    /// <param name="dateTimeUtc">The date and time of the event in UTC format.</param>
    private GroupEvent(User user, Name name, Category category, DateTime dateTimeUtc)
        : base(user, name, category, dateTimeUtc, EventType.GroupEvent)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEvent"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private GroupEvent()
    {
    }
    
    /// <summary>
    /// Navigation field.
    /// </summary>
    public ICollection<User> Attendees { get; set; }

    /// <summary>
    /// Gets or sets processed.
    /// </summary>
    public bool Processed { get; set; }
    
    /// <summary>
    /// Marks the event as processed and returns the respective result.
    /// </summary>
    /// <returns>The success result if the event was not previously marked as processed, otherwise a failure result.</returns>
    public Result MarkAsProcessed()
    {
        if (Processed)
        {
            return Result.Failure(DomainErrors.GroupEvent.AlreadyProcessed);
        }

        Processed = true;

        return Result.Success().GetAwaiter().GetResult();
    }
    
    /// <summary>
    /// Creates a new group event based on the specified parameters.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="name">The name.</param>
    /// <param name="category">The category.</param>
    /// <param name="dateTimeUtc">The date and time in UTC format.</param>
    /// <returns>The newly created group event.</returns>
    public static GroupEvent Create(User user, Name name, Category category, DateTime dateTimeUtc)
    {
        var groupEvent = new GroupEvent(user, name, category, dateTimeUtc);

        groupEvent.AddDomainEvent(new GroupEventCreatedDomainEvent(groupEvent));

        return groupEvent;
    }

    //TODO Create IDomainEventHandler where author and attendee will save group event and update for add attendees and work with them in bg tasks.
    //TODO In publisher updating group event.
    //TODO Create group event in publishers.
    //TODO Create bg task where group event sends emails.
    //TODO Thinking about Processed flag in group event and include him to group event entity.
    ///TODO When event cancelled send the notification author and attendees.
    
    /// <summary>
    /// Add to group event attendee.
    /// </summary>
    /// <param name="groupEvent">The group event.</param>
    /// <param name="attendee">The attendee</param>
    /// <returns>The newly created group event.</returns>
    public static async Task<Result> AddToGroupEventAttendee(GroupEvent groupEvent, User attendee)
    {
        if (!groupEvent.Cancelled)
        {
            groupEvent.AddDomainEvent(new AddToGroupEventAttendeeDomainEvent(groupEvent, attendee));

            return await Result.Success();
        }

        return Result.Failure(DomainErrors.GroupEvent.IsCancelled);
    }
    
    /// <inheritdoc />
    public override bool ChangeName(Name name)
    {
        string previousName = Name;

        bool hasChanged = base.ChangeName(name);

        if (hasChanged)
        {
            AddDomainEvent(new GroupEventNameChangedDomainEvent(this, previousName));
        }

        return hasChanged;
    }

    /// <inheritdoc />
    public override bool ChangeDateAndTime(DateTime dateTimeUtc)
    {
        DateTime previousDateAndTime = DateTimeUtc;

        bool hasChanged = base.ChangeDateAndTime(dateTimeUtc);

        if (hasChanged)
        {
            AddDomainEvent(new GroupEventDateAndTimeChangedDomainEvent(this, previousDateAndTime));
        }

        return hasChanged;
    }
}