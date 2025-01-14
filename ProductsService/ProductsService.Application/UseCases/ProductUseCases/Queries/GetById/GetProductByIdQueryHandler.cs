using AutoMapper;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetById
{
    public class GetProductByIdQueryHandler(
        IProductQueryRepository productRepository,
        IMapper mapper,
        IFileService fileService) : IQueryHandler<GetProductByIdQuery, ProductResponseDto>
    {
        public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product with such id doesnt exist");
            }

            foreach (var image in product.Images)
            {
                image.ImageUrl = fileService.GetFileUrl(image.ImageUrl);
            }

            return mapper.Map<ProductResponseDto>(product);
        }
    }
}
