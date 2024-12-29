using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetById
{
    public record GetProductByIdQuery(Guid ProductId) : ICachedQuery<ProductResponseDto>
    {
        public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(1);

        public TimeSpan? SlidingExpiration => TimeSpan.FromSeconds(30);

        public string Key => $"product:{ProductId}";
    }
}
