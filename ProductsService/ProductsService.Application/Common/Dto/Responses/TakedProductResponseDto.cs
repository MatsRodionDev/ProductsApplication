namespace ProductsService.Application.Common.Dto.Responses
{
    public record TakedProductResponseDto(
        Guid Id,
        string Name,
        double Price,
        int TakedQuantity,
        Guid UserId);
}
