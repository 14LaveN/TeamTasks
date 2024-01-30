using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Events.GroupEvent.Contracts.GroupEvents;

namespace TeamTasks.Events.GroupEvent.Events.Queries.Get10MostRecentAttendingGroupEvents;

/// <summary>
/// Represents the <see cref="Get10MostRecentAttendingGroupEventsQuery"/> handler.
/// </summary>
internal sealed class Get10MostRecentAttendingGroupEventsQueryHandler
    : IQueryHandler<Get10MostRecentAttendingGroupEventsQuery, Maybe<IReadOnlyCollection<GroupEventResponse>>>
{
    private readonly IDbContext<Domain.Entities.GroupEvent> _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="Get10MostRecentAttendingGroupEventsQueryHandler"/> class.
    /// </summary>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="dbContext">The database context.</param>
    public Get10MostRecentAttendingGroupEventsQueryHandler(IDbContext<Domain.Entities.GroupEvent> dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<IReadOnlyCollection<GroupEventResponse>>> Handle(
        Get10MostRecentAttendingGroupEventsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            return Maybe<IReadOnlyCollection<GroupEventResponse>>.None;
        }

        GroupEventResponse[] responses = await (
                from attendee in _dbContext.Set<Attendee>().AsNoTracking()
                join groupEvent in _dbContext.Set<Domain.Entities.GroupEvent>().AsNoTracking()
                    on attendee.EventId equals groupEvent.Id
                where attendee.UserId == request.UserId
                orderby groupEvent.DateTimeUtc
                select new GroupEventResponse
                {
                    Id = groupEvent.Id,
                    Name = groupEvent.Name.Value,
                    CategoryId = groupEvent.Category.Value,
                    DateTimeUtc = groupEvent.DateTimeUtc,
                    CreatedOnUtc = groupEvent.CreatedOnUtc
                })
            .Take(request.NumberOfGroupEventsToTake)
            .ToArrayAsync(cancellationToken);

        foreach (GroupEventResponse groupEventResponse in responses)
        {
            groupEventResponse.Category = Category.FromValue(groupEventResponse.CategoryId).Value.Name;
        }

        return responses;
    }
}