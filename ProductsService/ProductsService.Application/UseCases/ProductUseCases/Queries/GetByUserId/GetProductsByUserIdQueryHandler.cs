using AutoMapper;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetByUserId
{
    public class GetProductsByUserIdQueryHandler(
         IMapper mapper,
         IProductQueryRepository productRepository,
         IFileService fileService) : IQueryHandler<GetProductsByUserIdQuery, List<ProductResponseDto>>
    {
        public async Task<List<ProductResponseDto>> Handle(GetProductsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var filters = mapper.Map<GetUsersProductsFilters>(request);

            var products = await productRepository.GetByUserIdAndByFiltersAsync(filters, cancellationToken);

            products = GetProductsWithUsrls(products);

            return mapper.Map<List<ProductResponseDto>>(products);
        }

        private List<Product> GetProductsWithUsrls(List<Product> products)
        {
            foreach (var product in products)
            {
                foreach (var image in product.Images)
                {
                    image.ImageUrl = fileService.GetFileUrl(image.ImageUrl);
                }
            }

            return products;
        }
    }
}
