using MediatR;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetById
{
    public record GetProductByIdRequest(Guid ProductId) : ICachedQuery<ProductResponseDto>
    {
        public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(1);

        public TimeSpan? SlidingExpiration => TimeSpan.FromSeconds(30);

        public string Key => $"product:{ProductId}";
    }
}
