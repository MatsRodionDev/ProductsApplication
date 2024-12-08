using Microsoft.Extensions.Caching.Distributed;
using ProductsService.Application.Common.Interfaces.Services;
using System.Text.Json;

namespace ProductsService.Infrastructure.Services
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var jsonStringValue = await distributedCache.GetStringAsync(key, cancellationToken);

            if (jsonStringValue is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(jsonStringValue);
        }

        public async Task<TValue?> GetAsync<TKey, TValue>(TKey key, CancellationToken cancellationToken = default)
        {
            var jsonStringKey = JsonSerializer.Serialize(key);

            var jsonStringValue = await distributedCache.GetStringAsync(jsonStringKey, cancellationToken);

            if (jsonStringValue is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<TValue>(jsonStringValue);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null, CancellationToken cancellationToken = default)
        {
            var jsonStringValue = JsonSerializer.Serialize(value);

            var cacheOptions = new DistributedCacheEntryOptions();

            if(slidingExpiration is not null)
            {
                cacheOptions.SetSlidingExpiration((TimeSpan) slidingExpiration);
            }

            if (absoluteExpiration is not null)
            {
                cacheOptions.SetAbsoluteExpiration((TimeSpan) absoluteExpiration);
            }

            await distributedCache.SetStringAsync(key, jsonStringValue, cacheOptions, cancellationToken);
        }

        public async Task SetAsync<TKey, TValue>(TKey key, TValue value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null, CancellationToken cancellationToken = default)
        {
            var jsonStringKey = JsonSerializer.Serialize(key);
            var jsonStringValue = JsonSerializer.Serialize(value);

            var cacheOptions = new DistributedCacheEntryOptions();

            if (slidingExpiration is not null)
            {
                cacheOptions.SetSlidingExpiration((TimeSpan)slidingExpiration);
            }

            if (absoluteExpiration is not null)
            {
                cacheOptions.SetAbsoluteExpiration((TimeSpan)absoluteExpiration);
            }

            await distributedCache.SetStringAsync(jsonStringKey, jsonStringValue, cacheOptions, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            await distributedCache.RemoveAsync(key, cancellationToken);
        }

        public async Task RemoveAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            var jsonStringKey = JsonSerializer.Serialize(key);

            await distributedCache.RemoveAsync(jsonStringKey, cancellationToken);
        }
    }
}
