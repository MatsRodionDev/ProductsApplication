using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UserService.BLL.Interfaces.Services;

namespace UserService.BLL.Services
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

            var model = JsonSerializer.Deserialize<T>(jsonStringValue);

            return model;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default)
        {
            var jsonStringValue = JsonSerializer.Serialize(value);

            var options = new DistributedCacheEntryOptions();

            if(expirationTime != null)
            {
                options.SetAbsoluteExpiration(expirationTime.Value);
            }

            await distributedCache.SetStringAsync(
                key, 
                jsonStringValue,
                options,
                cancellationToken);
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            return distributedCache.RemoveAsync(key, cancellationToken);
        }
    }
}

