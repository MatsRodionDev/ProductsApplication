using AutoMapper;
using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Intefaces;
using OrderService.Application.Common.Models;
using ProductsService.API.Protos;


namespace OrderService.Infrastructure.Grpc
{
    public class ProductService(
        Products.ProductsClient grpcClient,
        IMapper mapper) : IProductService
    {
        public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var request = new ProductByIdRequest
            {
                Id = id.ToString()
            };

            var response = await grpcClient.GetProductAsync(request, cancellationToken: cancellationToken);

            return mapper.Map<Product>(response);
        }

        public async Task<TakedProduct> TakeProduct(TakeProductDto dto, CancellationToken cancellationToken)
        {
            var request = mapper.Map<ProductRequest>(dto);

            var response = await grpcClient.TakeOneProductAsync(request, cancellationToken: cancellationToken);

            return mapper.Map<TakedProduct>(response);
        }

        public async Task<List<TakedProduct>> TakeProducts(List<TakeProductDto> dtos, CancellationToken cancellationToken)
        {
            var request = new ProductsListRequest()
            {
                Products = { mapper.Map<List<ProductRequest>>(dtos) }
            };

            var response = await grpcClient.TakeProductsAsync(request, cancellationToken: cancellationToken);

            return mapper.Map<List<TakedProduct>>(response.Products);
        }
    }
}
