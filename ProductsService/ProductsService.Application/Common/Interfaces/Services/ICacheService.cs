namespace ProductsService.Application.Common.Interfaces.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task<TValue?> GetAsync<TKey, TValue>(TKey key, CancellationToken cancellationToken = default);
        Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null, CancellationToken cancellationToken = default);
        Task SetAsync<TKey, TValue>(TKey key, TValue value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null, CancellationToken cancellationToken = default);
        Task RemoveAsync(string key, CancellationToken cancellationToken);
        Task RemoveAsync<TKey>(TKey key, CancellationToken cancellationToken);
    }
}
