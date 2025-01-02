using ChatsService.DAL.Entities;

namespace ChatsService.DAL.Interfaces
{
    public interface IChatRepository : IGenericRepository<ChatEntity>
    {
        Task<ChatEntity?> GetByProductAndBuyerIdsAsync(Guid productId, Guid buyerId, CancellationToken cancellationToken = default);
        Task<List<ChatEntity>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default);
        Task<List<ChatEntity>> GetByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken = default);
    }
}
