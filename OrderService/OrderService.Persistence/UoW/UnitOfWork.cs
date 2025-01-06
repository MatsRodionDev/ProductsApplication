using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Interfaces;

namespace OrderService.Persistence.UoW
{
    public class UnitOfWork(
        ApplicationDbContext context,
        IBasketItemRepository basketItemRepository,
        IBasketRepository basketRepository,
        IOrderRepository orderRepository) : IUnitOfWork
    {
        public IBasketItemRepository BasketItemRepository => basketItemRepository; 
        public IBasketRepository BasketRepository => basketRepository;
        public IOrderRepository OrderRepository => orderRepository;
            
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        public void DataBaseMigrate()
        {
            if(context.Database.IsRelational())
            {
                context.Database.Migrate();
            }
        }
    }
}
