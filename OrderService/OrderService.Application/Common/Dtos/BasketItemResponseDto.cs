using OrderService.Application.Common.Models;

namespace OrderService.Application.Common.Dtos
{
    public record BasketItemResponseDto(
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal ToTalPrice,
        string Name,
        int AvailableQuantity,
        List<Image> Images);
}
