using OrderService.Domain.Models;

namespace OrderService.Domain.Interfaces
{
    public interface IBasketRepository : IGenericRepository<Basket>
    {
        Task<Basket?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Basket?> GetByUserIdWithTrackingAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
