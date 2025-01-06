using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using OrderService.Persistence.Specifications;

namespace OrderService.Persistence.Repositories
{
    public class BasketItemRepository(ApplicationDbContext context) : GenericRepository<BasketItem>(context), IBasketItemRepository
    { 
        public async Task<List<BasketItem>> GetByProductIdAndQuantityAsync(Guid productId, int Quantity, CancellationToken cancellationToken)
        {
            return await SpecificationEvaluator
                .GetQuery(
                    _dbSet.AsNoTracking(),
                    new GetBasketItemsByProductIdAndQuantitySpecification(productId, Quantity))
                .ToListAsync(cancellationToken);
                
        }

        public async Task<List<BasketItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(i => i.ProductId == productId)
                .ToListAsync(cancellationToken);
        }
    }
}
