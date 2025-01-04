using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Common.Intefaces
{
    public interface IProductService
    {
        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TakedProduct> TakeProduct(TakeProductDto dto, CancellationToken cancellationToken = default);
        Task<List<TakedProduct>> TakeProducts(List<TakeProductDto> dtos, CancellationToken cancellationToken = default);
    }
}
