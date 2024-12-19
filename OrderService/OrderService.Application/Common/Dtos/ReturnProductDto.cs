namespace OrderService.Application.Common.Dtos
{
    public record ReturnProductDto(
        Guid ProductId,
        int Quantity);
}
