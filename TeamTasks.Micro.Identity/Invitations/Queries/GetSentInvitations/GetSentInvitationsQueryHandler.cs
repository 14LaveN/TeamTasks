using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Application.Invitations.Queries.GetPendingInvitations;
using TeamTasks.Contracts.Invitations;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;
using TeamTasks.Database.Common;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Identity;

namespace TeamTasks.Application.Invitations.Queries.GetSentInvitations
{
    /// <summary>
    /// Represents the <see cref="GetPendingInvitationsQuery"/> handler.
    /// </summary>
    internal sealed class GetSentInvitationsQueryHandler
        : IQueryHandler<GetSentInvitationsQuery, Maybe<SentInvitationsListResponse>>
    {
        private readonly BaseDbContext<Invitation> _invitationDbContext;
        private readonly UserDbContext _userDbContext;
        private readonly BaseDbContext<GroupEvent> _groupEventDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSentInvitationsQueryHandler"/> class.
        /// </summary>
        /// <param name="invitationDbContext">The invitations database context.</param>
        /// <param name="userDbContext">The users database context.</param>
        /// <param name="groupEventDbContext">The group events database context.</param>
        public GetSentInvitationsQueryHandler(
            UserDbContext userDbContext,
            BaseDbContext<Invitation> invitationDbContext,
            BaseDbContext<GroupEvent> groupEventDbContext)
        {
            _userDbContext = userDbContext;
            _invitationDbContext = invitationDbContext;
            _groupEventDbContext = groupEventDbContext;
        }

        /// <inheritdoc />
        public async Task<Maybe<SentInvitationsListResponse>> Handle(
            GetSentInvitationsQuery request,
            CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return Maybe<SentInvitationsListResponse>.None;
            }

            SentInvitationsListResponse.SentInvitationModel[] invitations = await (
                from invitation in _invitationDbContext.Set<Invitation>().AsNoTracking()
                join friend in _userDbContext.Set<User>().AsNoTracking()
                    on invitation.UserId equals friend.Id
                join groupEvent in _groupEventDbContext.Set<GroupEvent>().AsNoTracking()
                    on invitation.EventId equals groupEvent.Id
                join user in _userDbContext.Set<User>().AsNoTracking()
                    on groupEvent.UserId equals user.Id
                where user.Id == request.UserId &&
                      groupEvent.UserId == request.UserId &&
                      invitation.CompletedOnUtc == null
                select new SentInvitationsListResponse.SentInvitationModel
                {
                    Id = invitation.Id,
                    FriendId = friend.Id,
                    FriendName = friend.FirstName.Value + " " + friend.LastName.Value,
                    EventName = groupEvent.Name.Value,
                    EventDateTimeUtc = groupEvent.DateTimeUtc,
                    CreatedOnUtc = invitation.CreatedOnUtc
                }).ToArrayAsync(cancellationToken);

            var response = new SentInvitationsListResponse(invitations);

            return response;
        }
    }
}
