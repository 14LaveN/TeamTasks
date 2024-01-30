using System.Linq.Expressions;
using TeamTasks.Database.Common.Specifications;

namespace TeamTasks.Database.Attendee.Data;

/// <summary>
/// Represents the specification for determining the unprocessed attendee.
/// </summary>
internal sealed class UnprocessedAttendeeSpecification : Specification<Domain.Entities.Attendee>
{
    /// <inheritdoc />
    public override Expression<Func<Domain.Entities.Attendee, bool>> ToExpression() => attendee => !attendee.Processed;
}