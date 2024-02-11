using Microsoft.IdentityModel.Tokens;
using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Helpers.JWT;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Database.Tasks.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Exceptions;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.DTO.Tasks;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Micro.TasksAPI.Commands.DoneTask;
using TeamTasks.Micro.TasksAPI.Contracts.Task.Get;

namespace TeamTasks.Micro.TasksAPI.Queries.GetAuthorTasksByIsDone;

/// <summary>
/// Represents the <see cref="GetAuthorTasksByIsDoneQueryHandler"/> class.
/// </summary>
public sealed class GetAuthorTasksByIsDoneQueryHandler
    : IQueryHandler<GetAuthorTasksByIsDoneQuery, Maybe<IEnumerable<GetTaskResponse>>>
{
    private readonly ITasksRepository _tasksRepository;
    private readonly ILogger<GetAuthorTasksByIsDoneQueryHandler> _logger;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetAuthorTasksByIsDoneQueryHandler"/> class.
    /// </summary>
    /// <param name="tasksRepository">The tasks repository.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="userRepository">The user repository.</param>
    public GetAuthorTasksByIsDoneQueryHandler(
        ITasksRepository tasksRepository,
        ILogger<GetAuthorTasksByIsDoneQueryHandler> logger,
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository)
    {
        _tasksRepository = tasksRepository;
        _logger = logger;
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
    }
    
    /// <inheritdoc />
    public async Task<Maybe<IEnumerable<GetTaskResponse>>> Handle(GetAuthorTasksByIsDoneQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for get tasks by author id - {request.UserId} {DateTime.UtcNow}");

            if (_userIdentifierProvider.UserId == Guid.Empty 
                && _userIdentifierProvider.UserId != request.UserId)
            {
                _logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }

            var tasks = await _tasksRepository
                .GetAuthorTasksByIsDone(_userIdentifierProvider.UserId);

            var tasksDtos = tasks.ToList();
            if (tasksDtos.IsNullOrEmpty())
            {
                _logger.LogWarning($"Your tasks not found by author identifier - {_userIdentifierProvider.UserId}");

                throw new NotFoundException(nameof(DomainErrors.Task.NotFound), DomainErrors.Task.NotFound);
            }
            
            _logger.LogInformation($"Get tasks by author - {request.UserId}");

            return Maybe<IEnumerable<GetTaskResponse>>.From(GetTaskResponse.ToGetTaskResponse(tasksDtos));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[GetAuthorTasksByIsDoneQueryHandler]: {exception.Message}");
            throw new ApplicationException();
        }
    }
}