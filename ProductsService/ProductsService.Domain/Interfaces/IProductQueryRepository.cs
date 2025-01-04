using ProductsService.Domain.Filters;
using ProductsService.Domain.Models;

namespace ProductsService.Domain.Interfaces
{
    public interface IProductQueryRepository 
    {
        Task<List<Product>> GetByUserIdAndByFiltersAsync(GetUsersProductsFilters filters, CancellationToken cancellationToken = default);
        Task<List<Product>> GetByFiltersAsync(GetProductsFilters filters, CancellationToken cancellationToken = default);
        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
