using OrderService.Domain.Filters;
using OrderService.Domain.Models;

namespace OrderService.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetByBuyerIdAsync(Guid BuyerId, CancellationToken cancellationToken = default);
        Task<List<Order>> GetBySellerIdAsync(Guid SellerId, CancellationToken cancellationToken = default);
        Task<List<Order>> GetOrdersByFiltersAsync(OrderFilters filters, CancellationToken cancellationToken);
    }
}
