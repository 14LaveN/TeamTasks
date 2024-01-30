using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Contracts.Attendees;
using TeamTasks.Database.Common;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Identity;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Micro.Identity.Attendees.Queries.GetAttendeesForEventId;

/// <summary>
/// Represents the <see cref="GetAttendeesForGroupEventIdQuery"/> handler.
/// </summary>
internal sealed class GetAttendeesForGroupEventIdQueryHandler
    : IQueryHandler<GetAttendeesForGroupEventIdQuery, Maybe<AttendeeListResponse>>
{
    private readonly BaseDbContext<Attendee> _dbContext;
    private readonly UserDbContext _userDbContext;
    private readonly BaseDbContext<GroupEvent> _groupEventDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAttendeesForGroupEventIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="userDbContext">The user database context.</param>
    /// <param name="groupEventDbContext">The group event database context.</param>
    public GetAttendeesForGroupEventIdQueryHandler(
        BaseDbContext<Attendee> dbContext,
        UserDbContext userDbContext,
        BaseDbContext<GroupEvent> groupEventDbContext)
    {
        _dbContext = dbContext;
        _userDbContext = userDbContext;
        _groupEventDbContext = groupEventDbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<AttendeeListResponse>> Handle(GetAttendeesForGroupEventIdQuery request,
        CancellationToken cancellationToken)
    {
        var gr = await (
            from attendee in _dbContext.Set<Attendee>().AsNoTracking()
            join groupEvent in _dbContext.Set<GroupEvent>().AsNoTracking()
                on attendee.EventId equals groupEvent.Id
            where groupEvent.Id == request.GroupEventId &&
                  !groupEvent.Cancelled &&
                  (groupEvent.UserId == request.UserId ||
                   attendee.UserId == request.UserId)
            select true).AnyAsync(cancellationToken);
        if (request.GroupEventId == Guid.Empty || gr)
        {
            return Maybe<AttendeeListResponse>.None;
        }

        AttendeeListResponse.AttendeeModel[] attendeeModels = await (
            from attendee in _dbContext.Set<Attendee>().AsNoTracking()
            join groupEvent in _groupEventDbContext.Set<GroupEvent>().AsNoTracking()
                on attendee.EventId equals groupEvent.Id
            join user in _userDbContext.Set<User>().AsNoTracking()
                on attendee.UserId equals user.Id
            where groupEvent.Id == request.GroupEventId && !groupEvent.Cancelled
            select new AttendeeListResponse.AttendeeModel
            {
                UserId = attendee.UserId,
                Name = user.FirstName.Value + " " + user.LastName.Value,
                CreatedOnUtc = attendee.CreatedOnUtc
            }).ToArrayAsync(cancellationToken);

        var response = new AttendeeListResponse(attendeeModels);

        return response;
    }
}