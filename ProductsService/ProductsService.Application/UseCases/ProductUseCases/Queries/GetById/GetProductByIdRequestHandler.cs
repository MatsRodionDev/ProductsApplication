using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using ProductsService.Application.Common;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetById
{
    public class GetProductByIdRequestHandler(
        IProductQueryRepository productRepository,
        IMapper mapper,
        IOptions<MinioOptions> options) : IQueryHandler<GetProductByIdRequest, ProductResponseDto>
    {
        private readonly MinioOptions _minioOptions = options.Value;

        public async Task<ProductResponseDto> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product with such id doesnt exist");
            }

            foreach (var image in product.Images)
            {
                image.ImageUrl = $"http://{_minioOptions.Endpoint}/{_minioOptions.BucketName}/{image.ImageName}";
            }

            return mapper.Map<ProductResponseDto>(product);
        }
    }
}
