using ProductsService.Domain.Filters;
using ProductsService.Domain.Models;
using ProductsService.Persistence.Specifications;
using ProductsService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProductsService.Persistence.Repositories
{
    public class ProductCommandRepository(ApplicationDBContext context) 
        : GenericRepository<Product>(context), 
        IProductCommandRepository
    {
        public async Task<Product?> GetByIdForUpdateAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await context.Products
                .FromSqlRaw(
                    """
                    SELECT * FROM "Products"
                    WHERE "Id" =  @id
                    FOR UPDATE
                    """, productId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Product>> GetByIdsAsync(List<Guid> productIds, CancellationToken cancellationToken = default)
        {
            return await context
                .Products
                .AsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Categories)
                .AsSplitQuery()
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetByIdsForUpdateAsync(List<Guid> productIds, CancellationToken cancellationToken = default)
        {
            return await context.Products
                .FromSqlRaw(
                    """
                    SELECT * FROM "Products"
                    WHERE "Id" = ANY (@ids) 
                    FOR UPDATE
                    """, productIds.ToArray())
                .ToListAsync(cancellationToken);
        }
    }
}
