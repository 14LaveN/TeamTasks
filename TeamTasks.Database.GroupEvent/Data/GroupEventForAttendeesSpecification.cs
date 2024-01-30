using System.Linq.Expressions;
using TeamTasks.Database.Common.Specifications;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.GroupEvent.Data;

/// <summary>
/// Represents the specification for determining the group event the attendees will be attending.
/// </summary>
public sealed class GroupEventForAttendeesSpecification : Specification<Domain.Entities.GroupEvent>
{
    private readonly Guid[] _groupEventIds;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEventForAttendeesSpecification"/> class.
    /// </summary>
    /// <param name="attendees">The attendees.</param>
    public GroupEventForAttendeesSpecification(IReadOnlyCollection<Attendee> attendees) =>
        _groupEventIds = attendees.Select(attendee => attendee.EventId).Distinct().ToArray();

    /// <inheritdoc />
    public override Expression<Func<Domain.Entities.GroupEvent, bool>> ToExpression() =>
        groupEvent => _groupEventIds.Contains(groupEvent.Id);
}