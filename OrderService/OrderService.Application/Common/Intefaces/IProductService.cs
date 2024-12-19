using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Common.Intefaces
{
    public interface IProductService
    {
        List<Product> TakeProducts(List<TakeProductDto> dtos);
        Product TakeProduct(TakeProductDto takeProduct);
        Product? GetByIdAsync(Guid id);
    }
}
