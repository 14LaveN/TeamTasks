using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.DTO.Tasks;
using TeamTasks.Micro.TasksAPI.Contracts.Task.Get;

namespace TeamTasks.Micro.TasksAPI.Queries.GetAuthorTasksByIsDone;

/// <summary>
/// Represents the get author tasks by is done command class.
/// </summary>
/// <param name="UserId">The user identifier.</param>>
public sealed record GetAuthorTasksByIsDoneQuery(Guid UserId)
    : ICachedQuery<Maybe<IEnumerable<GetTaskResponse>>>
{
    /// <summary>
    /// Create a new instance of the <see cref="GetAuthorTasksByIsDoneQuery"/> record class from user identifier.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The new <see cref="GetAuthorTasksByIsDoneQuery"/> record class.</returns>
    public static implicit operator GetAuthorTasksByIsDoneQuery(Guid userId) =>
        new(userId);

    /// <inheritdoc/>
    public string Key { get; } = $"get_author_tasks-{UserId}";

    /// <inheritdoc/>
    public TimeSpan? Expiration { get; } = TimeSpan.FromMinutes(5);
}