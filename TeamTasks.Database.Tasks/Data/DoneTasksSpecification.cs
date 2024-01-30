using System.Linq.Expressions;
using TeamTasks.Database.Common.Specifications;

namespace TeamTasks.Database.Tasks.Data;

/// <summary>
/// Represents the specification for determining the unprocessed personal event.
/// </summary>
public sealed class DoneTasksSpecification : Specification<Domain.Entities.TaskEntity>
{
    /// <inheritdoc />
    public override Expression<Func<Domain.Entities.TaskEntity, bool>> ToExpression() => taskEntity => taskEntity.IsDone;
}