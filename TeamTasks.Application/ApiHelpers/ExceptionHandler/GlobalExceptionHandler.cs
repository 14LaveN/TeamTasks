using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ErrorResponse = TeamTasks.Application.ApiHelpers.Responses.ErrorResponse;

namespace TeamTasks.Application.ApiHelpers.ExceptionHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred.");

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var errorResponse = new ErrorResponse()
        {
            Message = "An error occurred. Please try again later.",
            ErrorCode = "500"
        };

        var json = JsonSerializer.Serialize(errorResponse);
        await httpContext.Response.WriteAsJsonAsync(json, cancellationToken);

        return true;
    }
}