using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Helpers.JWT;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Database.Tasks.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;

namespace TeamTasks.Micro.TasksAPI.Commands.CreateTask;

/// <summary>
/// Represents the create task command handler class.
/// </summary>
public sealed class CreateTaskCommandHandler
    : ICommandHandler<CreateTaskCommand, IBaseResponse<Result>>
{
    private readonly ITasksRepository _tasksRepository;
    private readonly ILogger<CreateTaskCommandHandler> _logger;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTaskCommandHandler"/> class.
    /// </summary>
    /// <param name="tasksRepository">The tasks repository.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="userRepository">The user repository.</param>
    public CreateTaskCommandHandler(
        ITasksRepository tasksRepository,
        ILogger<CreateTaskCommandHandler> logger,
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository)
    {
        _tasksRepository = tasksRepository;
        _logger = logger;
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public async Task<IBaseResponse<Result>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for create the task - {request.Title} {DateTime.UtcNow}");

            Maybe<User> user = await _userRepository
                .GetByIdAsync(_userIdentifierProvider.UserId);

            if (user.HasNoValue)
            {
                _logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }
            
            var task = TaskEntity.Create(
                user.Value.Id,
                request.Priority,
                request.Description,
                request.Title,
                user.Value.CompanyId);

            await _tasksRepository.Insert(task);
            
            _logger.LogInformation($"Task created - {task.CreatedAt} {task.Title}");

            return new BaseResponse<Result>
            {
                Data = Result.Success(),
                StatusCode = StatusCode.Ok,
                Description = "Task created"
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[CreateTaskCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = StatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}