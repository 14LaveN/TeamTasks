using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace TeamTasks.Cache.Service;

public static class DistributedCacheExtensions
{
    //TODO Create the MemoryCacheExtensions
    public static async Task<T> GetOrCreateAsync<T>(
        this IMemoryCache cache,
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {

        T result = (await cache.GetOrCreateAsync<T>(key, entry =>
        {
            entry.SetAbsoluteExpiration(expiration ?? TimeSpan.FromMinutes(5));

            return factory(cancellationToken);
        }))!;

        return result;
    }
    
    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string recordId,
        T data,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = unusedExpireTime
        };

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }

    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache,
        string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);

        return jsonData is null ? default!
            : JsonSerializer.Deserialize<T>(jsonData)!;
    }
}