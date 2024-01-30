using System.Net;

namespace TeamTasks.Micro.Identity.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            //TODO var errorResponse = new ErrorResponse
            //TODO {
            //TODO     Message = "An error occurred. Please try again later.",
            //TODO     ErrorCode = "500"
            //TODO };
//TODO 
            //TODO var json = JsonSerializer.Serialize(errorResponse);
            //TODO await context.Response.WriteAsync(json);
        }
    }
}