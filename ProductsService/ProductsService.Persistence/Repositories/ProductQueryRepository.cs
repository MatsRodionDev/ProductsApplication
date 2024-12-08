using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using ProductsService.Persistence.Interfaces;
using ProductsService.Persistence.Specifications;

namespace ProductsService.Persistence.Repositories
{
    public class ProductQueryRepository : IProductQueryRepository
    {
        protected readonly IMongoContext _context;
        protected IMongoCollection<Product> _dbSet;

        public ProductQueryRepository(IMongoQueryContext context)
        {
            _context = context;

            _dbSet = _context.GetCollection<Product>(nameof(Product));
        }

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

        public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsQueryable()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }
}
