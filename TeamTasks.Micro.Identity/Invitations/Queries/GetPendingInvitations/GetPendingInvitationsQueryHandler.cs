using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Contracts.Invitations;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamTasks.Database.Common;
using TeamTasks.Database.Identity;

namespace TeamTasks.Application.Invitations.Queries.GetPendingInvitations
{
    /// <summary>
    /// Represents the <see cref="GetPendingInvitationsQuery"/> handler.
    /// </summary>
    internal sealed class GetPendingInvitationsQueryHandler
        : IQueryHandler<GetPendingInvitationsQuery, Maybe<PendingInvitationsListResponse>>
    {
        private readonly BaseDbContext<Invitation> _invitationDbContext;
        private readonly UserDbContext _userDbContext;
        private readonly BaseDbContext<GroupEvent> _groupEventDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPendingInvitationsQueryHandler"/> class.
        /// </summary>
        /// <param name="invitationDbContext">The invitations database context.</param>
        /// <param name="userDbContext">The users database context.</param>
        /// <param name="groupEventDbContext">The group events database context.</param>
        public GetPendingInvitationsQueryHandler(
            UserDbContext userDbContext,
            BaseDbContext<GroupEvent> groupEventDbContext,
            BaseDbContext<Invitation> invitationDbContext)
        {
            _userDbContext = userDbContext;
            _groupEventDbContext = groupEventDbContext;
            _invitationDbContext = invitationDbContext;
        }

        /// <inheritdoc />
        public async Task<Maybe<PendingInvitationsListResponse>> Handle(
            GetPendingInvitationsQuery request,
            CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return Maybe<PendingInvitationsListResponse>.None;
            }

            PendingInvitationsListResponse.PendingInvitationModel[] invitations = await (
                from invitation in _invitationDbContext.Set<Invitation>().AsNoTracking()
                join user in _userDbContext.Set<User>().AsNoTracking()
                    on invitation.UserId equals user.Id
                join groupEvent in _groupEventDbContext.Set<GroupEvent>().AsNoTracking()
                    on invitation.EventId equals groupEvent.Id
                join friend in _userDbContext.Set<User>().AsNoTracking()
                    on groupEvent.UserId equals friend.Id
                where invitation.UserId == request.UserId &&
                      invitation.CompletedOnUtc == null
                select new PendingInvitationsListResponse.PendingInvitationModel
                {
                    Id = invitation.Id,
                    FriendId = friend.Id,
                    FriendName = friend.FirstName.Value + " " + friend.LastName.Value,
                    EventName = groupEvent.Name.Value,
                    EventDateTimeUtc = groupEvent.DateTimeUtc,
                    CreatedOnUtc = invitation.CreatedOnUtc
                }).ToArrayAsync(cancellationToken);

            var response = new PendingInvitationsListResponse(invitations);

            return response;
        }
    }
}
