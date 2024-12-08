using ProductsService.Domain.Enums;

namespace ProductsService.Application.Common.Dto.Requests
{
    public record FiltersRequestDto(
        Categories? Category,
        string Name = "",
        OrderBy OrderBy = OrderBy.Id,
        bool Asc = false,
        int Page = 1,
        int PageSize = 5);
}
