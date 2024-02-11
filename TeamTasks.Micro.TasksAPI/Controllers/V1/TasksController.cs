using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.ApiHelpers.Contracts;
using TeamTasks.Application.ApiHelpers.Infrastructure;
using TeamTasks.Application.ApiHelpers.Policy;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.ValueObjects;
using TeamTasks.Micro.TasksAPI.Commands.CreateTask;
using TeamTasks.Micro.TasksAPI.Commands.DoneTask;
using TeamTasks.Micro.TasksAPI.Commands.UpdateTask;
using TeamTasks.Micro.TasksAPI.Contracts.Task.Create;
using TeamTasks.Micro.TasksAPI.Contracts.Task.Get;
using TeamTasks.Micro.TasksAPI.Queries.GetAuthorTasksByIsDone;

namespace TeamTasks.Micro.TasksAPI.Controllers.V1;

/// <summary>
/// Represents the task controller class.
/// </summary>
/// <param name="sender">The CQRS sender.</param>
/// <param name="userRepository">The <see cref="IUserRepository"/>.</param>
[Route("api/v1/tasks")]
public sealed class TasksController(
    ISender sender,
    IUserRepository userRepository)
    : ApiController(sender,userRepository)
{
    /// <summary>
    /// Create task.
    /// </summary>
    /// <param name="request">The <see cref="CreateTaskRequest"/> class.</param>
    /// <returns>Base information about create task method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">BadRequest.</response>
    /// <response code="500">Internal server error</response>
    [HttpPost(ApiRoutes.Task.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(taskRequest => new CreateTaskCommand(taskRequest.Title,
                taskRequest.Priority,
                taskRequest.Description))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, BadRequest);
    
    /// <summary>
    /// Done task.
    /// </summary>
    /// <param name="taskId">The task identifier.</param>
    /// <returns>Base information about done task method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">BadRequest.</response>
    /// <response code="500">Internal server error</response>
    [HttpPatch(ApiRoutes.Task.DoneTask)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DoneTask([FromRoute] Guid taskId) =>
        await Result.Create(taskId, DomainErrors.General.UnProcessableRequest)
            .Map(request => new DoneTaskCommand(request))
            .Ensure(request => request.TaskId == taskId, DomainErrors.General.UnProcessableRequest)
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, BadRequest);

    /// <summary>
    /// Update task.
    /// </summary>
    /// <param name="request">The <see cref="CreateTaskRequest"/> class.</param>
    /// <param name="taskId">The task identifier.</param>
    /// <returns>Base information about update task method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">BadRequest.</response>
    /// <response code="500">Internal server error</response>
    [HttpPost(ApiRoutes.Task.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTask([FromBody] CreateTaskRequest request,
        [FromRoute] Guid taskId) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(taskRequest => new UpdateTaskCommand(
                taskId,
                taskRequest.Title,
                taskRequest.Priority,
                taskRequest.Description))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, BadRequest);
    
    /// <summary>
    /// Get Author Tasks By tasks IsDone.
    /// </summary>
    /// <returns>Base information about get author tasks  method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">BadRequest.</response>
    /// <response code="500">Internal server error</response>
    [HttpGet(ApiRoutes.Task.GetAuthorTasksByIsDone)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAuthorTasksByIsDone() =>
        await Maybe<GetAuthorTasksByIsDoneQuery>
            .From(new GetAuthorTasksByIsDoneQuery(UserId))
            .Bind<GetAuthorTasksByIsDoneQuery, Maybe<IEnumerable<GetTaskResponse>>>(async query => await BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(query)))
            .Match(Ok, NotFound);
}