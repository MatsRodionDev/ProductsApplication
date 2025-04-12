namespace OrderService.Application.Common.Dtos
{
    public record TakeProductDto(
        Guid ProductId,
        int Quantity);
}
