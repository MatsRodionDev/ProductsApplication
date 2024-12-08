using ProductsService.Domain.Filters;
using ProductsService.Domain.Models;

namespace ProductsService.Domain.Interfaces
{
    public interface IProductCommandRepository
    {
        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Product>> GetByUserIdAndByFiltersAsync(GetUsersProductsFilters filters, CancellationToken cancellationToken = default);
        Task<List<Product>> GetByFiltersAsync(GetProductsFilters filters, CancellationToken cancellationToken = default);
    }
}
