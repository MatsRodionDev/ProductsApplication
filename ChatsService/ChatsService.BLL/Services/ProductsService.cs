using AutoMapper;
using ChatsService.BLL.Interfaces;
using ChatsService.BLL.Models;
using ChatsService.BLL.Protos;

namespace ChatsService.BLL.Services
{
    public class ProductsService(
        Products.ProductsClient grpcClient,
        IMapper mapper) : IProductService
    {
        public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await grpcClient.GetProductAsync(
                new ProductByIdRequest { Id = id.ToString() }, 
                cancellationToken: cancellationToken);

            return mapper.Map<Product>(product);
        }
    }
}
