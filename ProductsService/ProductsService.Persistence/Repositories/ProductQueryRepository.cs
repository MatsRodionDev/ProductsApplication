using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using ProductsService.Persistence.Specifications;

namespace ProductsService.Persistence.Repositories
{
    public class ProductQueryRepository(ApplicationDBContext context) : 
        GenericRepository<Product>(context), 
        IProductQueryRepository
    {
        public override async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Products
                .AsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
        public async Task<List<Product>> GetByUserIdAndByFiltersAsync(GetUsersProductsFilters filters, CancellationToken cancellationToken = default)
        {
            return await SpecificationEvaluator
                .GetQuery(
                    context.Products.AsNoTracking(),
                    new GetUsersProductsByFiltersSpecification(filters))
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetByFiltersAsync(GetProductsFilters filters, CancellationToken cancellationToken = default)
        {
            return await SpecificationEvaluator
                .GetQuery(
                    context.Products.AsNoTracking(),
                    new GetProductsByFiltersSpecification(filters))
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
