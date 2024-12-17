using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Persistence.Repositories
{
    public class BasketRepository(ApplicationDbContext context) :  GenericRepository<Basket>(context), IBasketRepository
    {
        public async Task<Basket?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(b => b.BasketItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);
        }

        public async Task<Basket?> GetByUserIdWithTrackingAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);
        }
    }
}
