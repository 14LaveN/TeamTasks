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