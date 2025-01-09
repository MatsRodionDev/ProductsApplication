using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Infrastructure.Interfaces
{
    public interface IUpdatableProductQueryRepository : IGenericRepository<Product>, IProductQueryRepository
    {
    }
}
