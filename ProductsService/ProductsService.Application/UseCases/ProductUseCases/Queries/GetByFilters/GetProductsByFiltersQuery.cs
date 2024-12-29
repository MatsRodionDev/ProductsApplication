using MediatR;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Domain.Enums;
using ProductsService.Domain.Models;


namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetByFilters
{
    public record GetProductsByFiltersQuery(
        Categories? Category,
        string ProductName = "",
        OrderBy OrderBy = OrderBy.Id,
        bool Asc = false,
        int Page = 1,
        int PageSize = 5) : IQuery<List<ProductResponseDto>>;

}
