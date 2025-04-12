namespace OrderService.Application.Common.Dtos
{
    public record CreateOrderRequestDto(
        Guid ProductId,
        int Quantity);
}
