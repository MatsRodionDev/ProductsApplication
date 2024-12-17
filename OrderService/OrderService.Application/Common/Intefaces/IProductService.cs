using OrderService.Application.Common.Models;

namespace OrderService.Application.Common.Intefaces
{
    public interface IProductService
    {
        void UpdateQuantity(Guid id, int quantity);
        Product? GetByIdAsync(Guid id);
    }
}
