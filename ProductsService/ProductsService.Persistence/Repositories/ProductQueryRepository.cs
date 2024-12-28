using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using ProductsService.Infrastructure.Interfaces;
using ProductsService.Persistence.Interfaces;
using ProductsService.Persistence.Specifications;

namespace ProductsService.Persistence.Repositories
{
    public class ProductQueryRepository(IMongoQueryContext context) : GenericRepository<Product>(context), IProductQueryRepository, IUpdatableProductQueryRepository
    {
        public async Task<List<Product>> GetByUserIdAndByFiltersAsync(GetUsersProductsFilters filters, CancellationToken cancellationToken = default)
        {
            return await SpecificationEvaluator
                .GetQuery(
                    _dbSet.AsQueryable(),
                    new GetUsersProductsByFiltersSpecification(filters))
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetByFiltersAsync(GetProductsFilters filters, CancellationToken cancellationToken = default)
        {
            return await SpecificationEvaluator
                .GetQuery(
                    _dbSet.AsQueryable(),
                    new GetProductsByFiltersSpecification(filters))
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
