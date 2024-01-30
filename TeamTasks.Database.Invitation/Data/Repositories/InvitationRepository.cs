using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;
using TeamTasks.Database.Common;
using TeamTasks.Database.Invitation.Data.Interfaces;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Invitation.Data.Repositories;

/// <summary>
/// Represents the invitation repository.
/// </summary>
internal sealed class InvitationRepository : GenericRepository<Domain.Entities.Invitation>, IInvitationRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvitationRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public InvitationRepository(BaseDbContext<Domain.Entities.Invitation> dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc />
    public async Task<bool> CheckIfInvitationAlreadySentAsync(GroupEvent groupEvent, User user) =>
        await AnyAsync(new PendingInvitationSpecification(groupEvent, user));

    /// <inheritdoc />
    public async Task RemoveInvitationsForGroupEventAsync(GroupEvent groupEvent, DateTime utcNow)
    {
        const string sql = @"
                UPDATE Invitation
                SET DeletedOnUtc = @DeletedOn, Deleted = @Deleted
                WHERE EventId = @EventId AND Deleted = 0";

        SqlParameter[] parameters =
        {
            new("@DeletedOn", utcNow),
            new("@Deleted", true),
            new("@EventId", groupEvent.Id)
        };

        await DbContext.ExecuteSqlAsync(sql, parameters);
    }
}