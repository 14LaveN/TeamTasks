using System.Text;
using Microsoft.Extensions.Caching.Distributed;

namespace TeamTasks.Micro.TasksAPI.Middlewares;

public class ResponseCachingMiddleware(
    RequestDelegate next,
    IDistributedCache cache)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Генерация ключа кэша на основе запроса
        var cacheKey = GenerateCacheKey(context.Request);

        // Попытка получить ответ из Redis cache
        var cachedResponse = await cache.GetAsync(cacheKey);

        if (cachedResponse is not null)
        {
            // Отправка закэшированного ответа
            await context.Response.Body.WriteAsync(cachedResponse, 0, cachedResponse.Length);
        }
        else
        {
            // Кэширование выполненного запроса
            var originalBodyStream = context.Response.Body;
            
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Выполнение последующих промежуточных ПО и контроллера
            await next(context);

            // Сохранение ответа в Redis cache
            await cache.SetAsync(cacheKey, responseBody.ToArray(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
            });
            
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private static string GenerateCacheKey(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append(request.Path);
        keyBuilder.Append(request.QueryString);

        return keyBuilder.ToString();
    }
}