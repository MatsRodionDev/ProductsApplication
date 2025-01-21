using AutoMapper;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetById
{
    public class GetProductByIdQueryHandler(
        IProductQueryRepository productRepository,
        IMapper mapper,
        IFileService fileService,
        ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdQuery, ProductResponseDto>
    {
        public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductByIdQuery for ProductId {ProductId}", request.ProductId);

            var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                logger.LogWarning("Product with id {ProductId} not found", request.ProductId);
                throw new NotFoundException("Product with such id doesnt exist");
            }

            foreach (var image in product.Images)
            {
                image.ImageUrl = fileService.GetFileUrl(image.ImageName);
            }

            var response = mapper.Map<ProductResponseDto>(product);

            logger.LogInformation("Successfully retrieved product with id {ProductId}", request.ProductId);

            return response;
        }
    }
}
