namespace OrderService.Application.Common.Dtos
{
    public record OrderResponseDto(
        Guid Id,
        Guid BuyerId,
        Guid SellerId,
        int Quantity,
        string Status,
        decimal ToTalPrice,
        OrderItemResponseDto OrderItem);
}
