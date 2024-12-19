using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Common.Intefaces
{
    public interface IProductService
    {
        void UpdateQuantity(TakeProductDto dto);
        void ReturnProduct(ReturnProductDto dto);
        Product? GetByIdAsync(Guid id);
    }
}
