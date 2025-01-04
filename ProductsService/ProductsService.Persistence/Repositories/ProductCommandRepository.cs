using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductsService.Persistence.Interfaces;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Models;
using ProductsService.Persistence.Specifications;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Persistence.Repositories
{
    public class ProductCommandRepository(IMongoCommandContext context) : GenericRepository<Product>(context), IProductCommandRepository
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

        public async Task<List<Product>> GetByFiltersAsync(GetProductsFilters filters,CancellationToken cancellationToken = default)
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
