using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace KRT.Services.Accounts.Infrastructure.CacheStorage;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var objectString = await _cache.GetStringAsync(key);

        if (string.IsNullOrWhiteSpace(objectString))
            return default;

        return JsonSerializer.Deserialize<T>(objectString);
    }

    public async Task RemoveAsync(string key) => await _cache.RemoveAsync(key);

    public async Task SetAsync<T>(string key, T data)
    {
        var memoryCacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
            SlidingExpiration = TimeSpan.FromSeconds(1200)
        };

        var objectString = JsonSerializer.Serialize(data);

        await _cache.SetStringAsync(key, objectString, memoryCacheEntryOptions);
    }
}
