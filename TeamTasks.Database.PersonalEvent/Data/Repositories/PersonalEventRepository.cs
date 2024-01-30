using Microsoft.EntityFrameworkCore;
using TeamTasks.Database.Common;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Common.Specifications;
using TeamTasks.Database.PersonalEvent.Data.Interfaces;

namespace TeamTasks.Database.PersonalEvent.Data.Repositories;

/// <summary>
/// Represents the attendee repository.
/// </summary>
internal sealed class PersonalEventRepository : GenericRepository<Domain.Entities.PersonalEvent>, IPersonalEventRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalEventRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PersonalEventRepository(BaseDbContext<Domain.Entities.PersonalEvent> dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Domain.Entities.PersonalEvent>> GetUnprocessedAsync(int take) =>
        await DbContext.Set<Domain.Entities.PersonalEvent>()
            .Where(new UnProcessedPersonalEventSpecification())
            .OrderBy(personalEvent => personalEvent.CreatedOnUtc)
            .Take(take)
            .ToArrayAsync();
}