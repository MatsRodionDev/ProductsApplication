using ProductsService.Domain.Models;

namespace ProductsService.Domain.Interfaces
{
    public interface IOutboxRepository 
    {
        Task<List<Outbox>> GetForUpdateAsync(int batchSize, CancellationToken cancellationToken = default);
        Task AddAsync<T>(T eventModel, CancellationToken cancellationToken = default);
        void UpdateMany(List<Outbox> outboxes);
    }
}
