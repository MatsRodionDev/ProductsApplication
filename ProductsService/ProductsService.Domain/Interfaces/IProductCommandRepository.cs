using ProductsService.Domain.Models;

namespace ProductsService.Domain.Interfaces
{
    public interface IProductCommandRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByIdForUpdateAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<List<Product>> GetByIdsForUpdateAsync(List<Guid> productIds, CancellationToken cancellationToken = default);
        Task<List<Product>> GetByIdsAsync(List<Guid> productIds, CancellationToken cancellationToken = default);
    }
}
