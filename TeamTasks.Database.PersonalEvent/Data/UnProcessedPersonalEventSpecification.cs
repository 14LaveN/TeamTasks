using System.Linq.Expressions;
using TeamTasks.Database.Common.Specifications;

namespace TeamTasks.Database.PersonalEvent.Data;

/// <summary>
/// Represents the specification for determining the unprocessed personal event.
/// </summary>
public sealed class UnProcessedPersonalEventSpecification : Specification<Domain.Entities.PersonalEvent>
{
    /// <inheritdoc />
    public override Expression<Func<Domain.Entities.PersonalEvent, bool>> ToExpression() => personalEvent => !personalEvent.Processed;
}