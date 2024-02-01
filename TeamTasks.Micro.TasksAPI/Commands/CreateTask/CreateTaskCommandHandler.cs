using Azure.Core;
using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Helpers.JWT;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Tasks.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Micro.TasksAPI.Contracts.Task.Create;

namespace TeamTasks.Micro.TasksAPI.Commands.CreateTask;

public sealed class CreateTaskCommandHandler
    : ICommandHandler<CreateTaskCommand, IBaseResponse<Result>>
{
    private readonly ITasksRepository _tasksRepository;
    private readonly ILogger<CreateTaskCommandHandler> _logger;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTaskCommandHandler"/> class.
    /// </summary>
    /// <param name="tasksRepository">The tasks repository.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    public CreateTaskCommandHandler(
        ITasksRepository tasksRepository,
        ILogger<CreateTaskCommandHandler> logger,
        IUserIdentifierProvider userIdentifierProvider)
    {
        _tasksRepository = tasksRepository;
        _logger = logger;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<IBaseResponse<Result>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for create a task - {request.Title} {DateTime.UtcNow}");

            if (_userIdentifierProvider.UserId == Guid.Empty && _userIdentifierProvider.UserId != request.AuthorId)
            {
                _logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }
            
            
            
            var task = TaskEntity.Create(
                _userIdentifierProvider.UserId,
                request.Priority,
                request.Description,
                request.Title);

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