using OrderService.Domain.Models;

namespace OrderService.Domain.Interfaces
{
    public interface IBasketItemRepository : IGenericRepository<BasketItem>
    {
        Task<List<BasketItem>> GetByProductIdAndQuantityAsync(Guid productId, int Quantity, CancellationToken cancellationToken = default);
        Task<List<BasketItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    }
}
