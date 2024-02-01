namespace TeamTasks.Micro.TasksAPI.Middlewares;

public static class MiddlewareConfiguration
{
    public static WebApplication UseCustomMiddlewares(this WebApplication app)
    {
        if (app is null)
            throw new ArgumentException();
        
        app.UseMiddleware<RequestLoggingMiddleware>(app.Logger);
        app.UseMiddleware<ResponseCachingMiddleware>();

        return app;
    }
}