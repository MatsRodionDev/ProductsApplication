using ProductsService.Domain.Enums;

namespace ProductsService.Application.Common.Dto.Requests
{
    public record CreateCategoryToProductRequestDto(
        Categories Category,
        Guid UserId);
}
