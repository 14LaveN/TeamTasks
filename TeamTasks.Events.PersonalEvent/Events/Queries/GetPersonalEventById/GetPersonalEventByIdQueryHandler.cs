using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Identity;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Events.PersonalEvent.Contracts.PersonalEvents;

namespace TeamTasks.Events.PersonalEvent.Events.Queries.GetPersonalEventById;

/// <summary>
/// Represents the <see cref="GetPersonalEventByIdQuery"/> handler.
/// </summary>
internal sealed class GetPersonalEventByIdQueryHandler : IQueryHandler<GetPersonalEventByIdQuery, Maybe<DetailedPersonalEventResponse>>
{
    private readonly IDbContext<Domain.Entities.PersonalEvent> _dbContext;
    private readonly UserDbContext _userDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonalEventByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="userDbContext"></param>
    public GetPersonalEventByIdQueryHandler(
        IDbContext<Domain.Entities.PersonalEvent> dbContext,
        UserDbContext userDbContext)
    {
        _dbContext = dbContext;
        _userDbContext = userDbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<DetailedPersonalEventResponse>> Handle(
        GetPersonalEventByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.PersonalEventId == Guid.Empty)
        {
            return Maybe<DetailedPersonalEventResponse>.None;
        }

        DetailedPersonalEventResponse? response = await (
            from personalEvent in _dbContext.Set<Domain.Entities.PersonalEvent>().AsNoTracking()
            join user in _userDbContext.Set<User>().AsNoTracking()
                on personalEvent.UserId equals user.Id
            where user.Id == request.UserId &&
                  personalEvent.Id == request.PersonalEventId &&
                  !personalEvent.Cancelled
            select new DetailedPersonalEventResponse
            {
                Id = personalEvent.Id,
                Name = personalEvent.Name.Value,
                CategoryId = personalEvent.Category.Value,
                CreatedBy = user.FirstName.Value + " " + user.LastName.Value,
                DateTimeUtc = personalEvent.DateTimeUtc,
                CreatedOnUtc = personalEvent.CreatedOnUtc
            }).FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            return Maybe<DetailedPersonalEventResponse>.None;
        }

        response.Category = Category.FromValue(response.CategoryId).Value.Name;
            
        return response;
    }
}