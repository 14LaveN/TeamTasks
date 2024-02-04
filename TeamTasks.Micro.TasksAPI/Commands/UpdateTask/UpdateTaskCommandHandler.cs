using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Helpers.JWT;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Database.Tasks.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Exceptions;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Micro.TasksAPI.Commands.CreateTask;

namespace TeamTasks.Micro.TasksAPI.Commands.UpdateTask;

/// <summary>
///  Represents the update task command handler class.
/// </summary>
internal sealed class UpdateTaskCommandHandler
    : ICommandHandler<UpdateTaskCommand, IBaseResponse<Result>>
{
    private readonly ITasksRepository _tasksRepository;
    private readonly ILogger<UpdateTaskCommandHandler> _logger;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTaskCommandHandler"/> class.
    /// </summary>
    /// <param name="tasksRepository">The tasks repository.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="userRepository">The user repository.</param>
    public UpdateTaskCommandHandler(
        ITasksRepository tasksRepository,
        ILogger<UpdateTaskCommandHandler> logger,
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository)
    {
        _tasksRepository = tasksRepository;
        _logger = logger;
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
    }
    
    /// <inheritdoc />
    public async Task<IBaseResponse<Result>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for update the task - {request.Title} {DateTime.UtcNow}");

            Maybe<User> user = await _userRepository
                .GetByIdAsync(_userIdentifierProvider.UserId);

            if (user.HasNoValue)
            {
                _logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }

            Maybe<TaskEntity> maybeTask = await _tasksRepository
                .GetByIdAsync(request.TaskId);

            if (maybeTask.HasNoValue)
            {
                _logger.LogWarning($"Task with the same identifier - {request.TaskId} not found");
                throw new NotFoundException(nameof(DomainErrors.Task.NotFound), DomainErrors.Task.NotFound);
            }

            TaskEntity task = maybeTask;
            task.Title = request.Title;
            task.Description = request.Description;
            task.Priority = request.Priority;
            
            Result result = await _tasksRepository.UpdateTask(task);
            
            if (result.IsSuccess)
                _logger.LogInformation($"Task updated - {task.CreatedAt} {task.Title}");

            return new BaseResponse<Result>
            {
                Data = Result.Success(),
                StatusCode = StatusCode.Ok,
                Description = "Task updated"
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[UpdateTaskCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = StatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}