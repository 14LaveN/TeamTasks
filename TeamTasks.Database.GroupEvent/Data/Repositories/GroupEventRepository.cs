using Microsoft.EntityFrameworkCore;
using TeamTasks.Database.Common;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Common.Specifications;
using TeamTasks.Database.GroupEvent.Data.Interfaces;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.GroupEvent.Data.Repositories;

/// <summary>
/// Represents the group event repository.
/// </summary>
internal sealed class GroupEventRepository : GenericRepository<Domain.Entities.GroupEvent>, IGroupEventRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEventRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public GroupEventRepository(BaseDbContext<Domain.Entities.GroupEvent> dbContext)
        : base(dbContext) { }
    
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<Domain.Entities.GroupEvent>> GetUnprocessedAsync(int take) =>
        await DbContext.Set<Domain.Entities.GroupEvent>()
            .Where(new UnProcessedGroupEventSpecification())
            .OrderBy(personalEvent => personalEvent.CreatedOnUtc)
            .Take(take)
            .ToArrayAsync();

    /// <inheritdoc/>
    public async Task<Maybe<Domain.Entities.GroupEvent>> GetGroupEventByName(string name) =>
        (await DbContext.Set<Domain.Entities.GroupEvent>().FirstOrDefaultAsync(x => x.Name == name))!;
}