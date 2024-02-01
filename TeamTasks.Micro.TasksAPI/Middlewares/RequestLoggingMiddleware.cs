namespace TeamTasks.Micro.TasksAPI.Middlewares;

/// <summary>
/// The request logging middleware.
/// </summary>
/// <param name="logger">The logger.</param>
/// <param name="next">The request delegate.</param>
public class RequestLoggingMiddleware(
    ILogger<RequestLoggingMiddleware> logger,
    RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation("Get request: {Request}", context.Request);

        await next.Invoke(context);
    }
}