namespace ProductsService.Application.Common.Dto.Responses
{
    public record ProductResponseDto(
        Guid Id,
        string Name,
        string Description,
        int Quantity,
        int Price,
        Guid UserId,
        List<CategoryResponseDto> Categories,
        List<ImageResponseDto> Images);
}
