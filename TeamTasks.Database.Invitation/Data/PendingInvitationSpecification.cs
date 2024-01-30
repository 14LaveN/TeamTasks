using System.Linq.Expressions;
using TeamTasks.Database.Common.Specifications;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Invitation.Data;

/// <summary>
/// Represents the specification for determining the pending invitation.
/// </summary>
internal sealed class PendingInvitationSpecification : Specification<Domain.Entities.Invitation>
{
    private readonly Guid _groupEventId;
    private readonly Guid _userId;

    internal PendingInvitationSpecification(GroupEvent groupEvent, User user)
    {
        _groupEventId = groupEvent.Id;
        _userId = user.Id;
    }

    /// <inheritdoc />
    public override Expression<Func<Domain.Entities.Invitation, bool>> ToExpression() =>
        invitation => invitation.CompletedOnUtc == null &&
                      invitation.EventId == _groupEventId &&
                      invitation.UserId == _userId;
}