using ProductsService.Domain.Enums;

namespace ProductsService.Domain.Filters
{
    public record GetProductsFilters(
        string? Category,
        string ProductName,
        OrderBy OrderBy,
        bool Asc,
        int Page = 1,
        int PageSize = 5);
}
