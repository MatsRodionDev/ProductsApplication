using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Persistence.Repositories
{
    public class BasketItemRepository(ApplicationDbContext context) : GenericRepository<BasketItem>(context), IBasketItemRepository
    { 
    }
}
