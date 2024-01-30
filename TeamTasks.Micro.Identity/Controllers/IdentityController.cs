using System.Diagnostics;
using TeamTasks.Micro.Identity.Commands.Login;
using TeamTasks.Micro.Identity.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.Core.Helpers.Metric;
using TeamTasks.Database.Identity;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Micro.Identity.Controllers;

[Route("api/v1/identity")]
public class IdentityController(ISender mediator,
        IUserUnitOfWork unitOfWork,
        CreateMetricsHelper createMetricsHelper)
    : ApiBaseController
{
    /// <summary>
    /// Login account.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Base information about register an account</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">Return description and access token</response>
    /// <response code="400"></response>
    /// <response code="500">Internal server error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("login-user")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand request)
    {
        //TODO Work with metrics to MetricsBehaviour in MediatR.
        var stopWatch = Stopwatch.StartNew();
        
        var response = await mediator.Send(request);
        
        stopWatch.Stop();
        await createMetricsHelper.CreateMetrics(stopWatch);
        
        if (response.StatusCode == Domain.Enumerations.StatusCode.Ok)
        {
            return Ok(new
            {
                description = response.Description,
                accessToken = response.AccessToken
            });
        }
        return BadRequest(new { descritpion = response.Description});
    }
    
    /// <summary>
    /// Register account
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Base information about login an account</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">Return description and access token</response>
    /// <response code="400"></response>
    /// <response code="500">Internal server error</response>
    [HttpPost("register-user")]
    public async Task<User> Register(
        [FromBody] RegisterCommand request)
    {
        var stopWatch = Stopwatch.StartNew();
        
        var response = await mediator.Send(request);
        
        stopWatch.Stop();
        await createMetricsHelper.CreateMetrics(stopWatch);
        
        if (response.StatusCode == Domain.Enumerations.StatusCode.Ok)
        {
            return response.Data!;
        }

        throw new AggregateException(response.Description);
    }
    
    [HttpGet("get-profile")]
    public string? GetProfile()
    {
        var name = User.Claims.FirstOrDefault(x=> x.Type == "name")?.Value;
        return name;
    }
}