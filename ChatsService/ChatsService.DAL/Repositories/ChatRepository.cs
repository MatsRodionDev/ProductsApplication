using ChatsService.DAL.Entities;
using ChatsService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatsService.DAL.Repositories
{
    public class ChatRepository(ApplicationDbContext context) : GenericRepository<ChatEntity>(context), IChatRepository
    {
        public async Task<List<ChatEntity>> GetByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.Messages.OrderByDescending(m => m.CreatedAt))
                .AsNoTracking()
                .Where(c => c.BuyerId == buyerId)
                .OrderByDescending(c => c.UpdatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<ChatEntity?> GetByProductAndBuyerIdsAsync(Guid productId, Guid buyerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(c => c.ProductId == productId)
                .Where(c => c.BuyerId == buyerId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<ChatEntity>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.Messages.OrderByDescending(m => m.CreatedAt))
                .AsNoTracking()
                .Where(c => c.SellerId == sellerId)
                .OrderByDescending(c => c.UpdatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
