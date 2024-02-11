using System.Linq.Expressions;
using TeamTasks.Database.Common.Specifications;

namespace TeamTasks.Database.GroupEvent.Data;

/// <summary>
/// Represents the specification for determining the unprocessed group event.
/// </summary>
public sealed class UnProcessedGroupEventSpecification : Specification<Domain.Entities.GroupEvent>
{
    /// <inheritdoc />
    public override Expression<Func<Domain.Entities.GroupEvent, bool>> ToExpression() => groupEvent => !groupEvent.Processed;
}