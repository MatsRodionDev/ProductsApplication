using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Filters;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using OrderService.Persistence.Specifications;

namespace OrderService.Persistence.Repositories
{
    public class OrderRepository(ApplicationDbContext context) : GenericRepository<Order>(context), IOrderRepository
    {
        public override async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(o => o.OrderItem)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<List<Order>> GetBySellerIdAsync(Guid SellerId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(o => o.OrderItem)
                .AsNoTracking()
                .Where(o => o.SellerId == SellerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Order>> GetByBuyerIdAsync(Guid BuyerId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(o => o.OrderItem)
                .AsNoTracking()
                .Where(o => o.BuyerId == BuyerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Order>> GetOrdersByFiltersAsync(OrderFilters filters, CancellationToken cancellationToken)
        {
            return await SpecificationEvaluator
                .GetQuery(
                    _dbSet.AsNoTracking(), 
                    new GetOrdersByFiltersSpecification(filters))
                .Skip((filters.Page -1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
