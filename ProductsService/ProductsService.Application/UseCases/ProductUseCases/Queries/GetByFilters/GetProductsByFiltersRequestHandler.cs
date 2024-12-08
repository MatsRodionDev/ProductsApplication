using AutoMapper;
using Microsoft.Extensions.Options;
using ProductsService.Application.Common;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetByFilters
{
    public class GetProductsByFiltersRequestHandler(
        IMapper mapper,
        IProductQueryRepository productRepository,
        IOptions<MinioOptions> options) : IQueryHandler<GetProductsByFiltersRequest, List<ProductResponseDto>>
    {
        private readonly MinioOptions _minioOptions = options.Value;

        public async Task<List<ProductResponseDto>> Handle(GetProductsByFiltersRequest request, CancellationToken cancellationToken)
        {
            var filters = mapper.Map<GetProductsFilters>(request);

            var products = await productRepository.GetByFiltersAsync(filters, cancellationToken);

            products = GetProductsWithUsrls(products);

            return mapper.Map<List<ProductResponseDto>>(products);
        }

        private List<Product> GetProductsWithUsrls(List<Product> products)
        {
            foreach (var product in products)
            {
                foreach (var image in product.Images)
                {
                    image.ImageUrl = $"http://{_minioOptions.Endpoint}/{_minioOptions.BucketName}/{image.ImageName}";
                }
            }

            return products;
        }
    }
}
