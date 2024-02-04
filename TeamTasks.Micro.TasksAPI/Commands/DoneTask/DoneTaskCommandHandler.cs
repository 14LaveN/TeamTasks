using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Helpers.JWT;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Database.Tasks.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Exceptions;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Micro.TasksAPI.Commands.CreateTask;

namespace TeamTasks.Micro.TasksAPI.Commands.DoneTask;

/// <summary>
/// Represents the done task command handler class.
/// </summary>
public sealed class DoneTaskCommandHandler
    : ICommandHandler<DoneTaskCommand, IBaseResponse<Result>>
{
    private readonly ITasksRepository _tasksRepository;
    private readonly ILogger<DoneTaskCommandHandler> _logger;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DoneTaskCommandHandler"/> class.
    /// </summary>
    /// <param name="tasksRepository">The tasks repository.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="userRepository">The user repository.</param>
    public DoneTaskCommandHandler(
        ITasksRepository tasksRepository,
        ILogger<DoneTaskCommandHandler> logger,
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository)
    {
        _tasksRepository = tasksRepository;
        _logger = logger;
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
    }
    
    /// <inheritdoc />
    public async Task<IBaseResponse<Result>> Handle(DoneTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for done task by id - {request.TaskId} {DateTime.UtcNow}");

            if (_userIdentifierProvider.UserId == Guid.Empty)
            {
                _logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }

            var task = await _tasksRepository
                .GetByIdAsync(request.TaskId);

            var user = await _userRepository
                .GetByIdAsync(_userIdentifierProvider.UserId);

            if (task.HasNoValue)
            {
                _logger.LogWarning($"Your task not found by id - {request.TaskId}");

                throw new NotFoundException(nameof(DomainErrors.Task.NotFound), DomainErrors.Task.NotFound);
            }

            var doneTaskResult = await task.Value.DoneTask(task, user);

            if (!doneTaskResult.IsSuccess)
            {
                _logger.LogWarning(DomainErrors.Task.AlreadyDone + $"by the id-{task.Value.Id}");
                throw new Exception(DomainErrors.Task.AlreadyDone);
            }
            
            _logger.LogInformation($"Done task - {task.Value.CreatedAt} {task.Value.Title}");

            return new BaseResponse<Result>
            {
                Data = Result.Success(),
                StatusCode = StatusCode.Ok,
                Description = "Done task"
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[DoneTaskCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = StatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}
