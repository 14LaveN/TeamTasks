using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Contracts.Common;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Identity;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Events.PersonalEvent.Contracts.PersonalEvents;

namespace TeamTasks.Events.PersonalEvent.Events.Queries.GetPersonalEvents;

/// <summary>
/// Represents the <see cref="GetPersonalEventsQuery"/> handler.
/// </summary>
internal sealed class GetPersonalEventsQueryHandler : IQueryHandler<GetPersonalEventsQuery, Maybe<PagedList<PersonalEventResponse>>>
{
    private readonly IDbContext<Domain.Entities.PersonalEvent> _dbContext;
    private readonly UserDbContext _userDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonalEventsQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="userDbContext"></param>
    public GetPersonalEventsQueryHandler(
        IDbContext<Domain.Entities.PersonalEvent> dbContext,
        UserDbContext userDbContext)
    {
        _dbContext = dbContext;
        _userDbContext = userDbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<PagedList<PersonalEventResponse>>> Handle(
        GetPersonalEventsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            return Maybe<PagedList<PersonalEventResponse>>.None;
        }

        bool shouldSearchCategory = request.CategoryId != null && Category.ContainsValue(request.CategoryId.Value);

        IQueryable<PersonalEventResponse> personalEventResponses =
            from personalEvent in _dbContext.Set<Domain.Entities.PersonalEvent>().AsNoTracking()
            join user in _userDbContext.Set<User>().AsNoTracking()
                on personalEvent.UserId equals user.Id
            where user.Id == request.UserId &&
                  personalEvent.UserId == request.UserId &&
                  !personalEvent.Cancelled &&
                  (!shouldSearchCategory || personalEvent.Category.Value == request.CategoryId) &&
                  (request.Name == null || request.Name == "" || personalEvent.Name.Value.Contains(request.Name)) &&
                  (request.StartDate == null || personalEvent.DateTimeUtc >= request.StartDate) &&
                  (request.EndDate == null || personalEvent.DateTimeUtc <= request.EndDate)
            orderby personalEvent.DateTimeUtc descending
            select new PersonalEventResponse
            {
                Id = personalEvent.Id,
                Name = personalEvent.Name.Value,
                CategoryId = personalEvent.Category.Value,
                DateTimeUtc = personalEvent.DateTimeUtc,
                CreatedOnUtc = personalEvent.CreatedOnUtc
            };

        int totalCount = await personalEventResponses.CountAsync(cancellationToken);

        PersonalEventResponse[] personalEventResponsesPage = await personalEventResponses
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        foreach (PersonalEventResponse personalEventResponse in personalEventResponsesPage)
        {
            personalEventResponse.Category = Category.FromValue(personalEventResponse.CategoryId).Value.Name;
        }

        return new PagedList<PersonalEventResponse>(personalEventResponsesPage, request.Page, request.PageSize, totalCount);
    }
}