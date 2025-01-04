using OrderService.Domain.Enums;

namespace OrderService.Application.Common.Dtos
{
    public record UpdateOrderStatusRequestDto(
        OrderStatus Status);
}
