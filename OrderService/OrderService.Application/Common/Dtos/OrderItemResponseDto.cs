namespace OrderService.Application.Common.Dtos
{
    public record OrderItemResponseDto(
        Guid ProductId,
        string ProdcutName,
        decimal ProductPrice);
}
