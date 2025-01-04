namespace OrderService.Application.Common.Dtos
{
    public record BasketResponseDto(
        Guid UserId,
        List<BasketItemResponseDto?> BasketItems);
}
