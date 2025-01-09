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


        public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            await distributedCache.RemoveAsync(key, cancellationToken);
        }

    }
}
