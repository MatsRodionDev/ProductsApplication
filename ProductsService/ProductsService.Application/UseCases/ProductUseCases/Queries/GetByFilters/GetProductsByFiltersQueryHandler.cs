using AutoMapper;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetByFilters
{
    public class GetProductsByFiltersQueryHandler(
        IMapper mapper,
        IProductQueryRepository productRepository,
        IFileService fileService,
        ILogger<GetProductsByFiltersQueryHandler> logger) : IQueryHandler<GetProductsByFiltersQuery, List<ProductResponseDto>>
    {
        public async Task<List<ProductResponseDto>> Handle(GetProductsByFiltersQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductsByFiltersQuery");

            var filters = mapper.Map<GetProductsFilters>(request);

            var products = await productRepository.GetByFiltersAsync(filters, cancellationToken);

            products = GetProductsWithUrls(products);

            var response = mapper.Map<List<ProductResponseDto>>(products);

            logger.LogInformation("Successfully retrieved {ProductCount} products", response.Count);

            return response;
        }

        private List<Product> GetProductsWithUrls(List<Product> products)
        {
            foreach (var product in products)
            {
                foreach (var image in product.Images)
                {
                    image.ImageUrl = fileService.GetFileUrl(image.ImageName);
                }
            }

            return products;
        }
    }
}
