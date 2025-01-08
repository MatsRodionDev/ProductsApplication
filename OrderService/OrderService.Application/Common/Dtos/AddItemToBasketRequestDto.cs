namespace OrderService.Application.Common.Dtos
{
    public record AddItemToBasketRequestDto(
        Guid ProductId,
        int Quantity);
}
