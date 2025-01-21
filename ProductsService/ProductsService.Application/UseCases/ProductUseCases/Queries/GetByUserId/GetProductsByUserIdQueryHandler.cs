using AutoMapper;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetByUserId
{
    public class GetProductsByUserIdQueryHandler(
         IMapper mapper,
         IProductQueryRepository productRepository,
         IFileService fileService,
         ILogger<GetProductsByUserIdQueryHandler> logger) : IQueryHandler<GetProductsByUserIdQuery, List<ProductResponseDto>>
    {
        public async Task<List<ProductResponseDto>> Handle(GetProductsByUserIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductsByUserIdQuery for UserId {UserId}", request.UserId);

            var filters = mapper.Map<GetUsersProductsFilters>(request);

            var products = await productRepository.GetByUserIdAndByFiltersAsync(filters, cancellationToken);

            products = GetProductsWithUrls(products);

            var response = mapper.Map<List<ProductResponseDto>>(products);

            logger.LogInformation("Successfully retrieved {ProductCount} products for UserId {UserId}", response.Count, request.UserId);

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
