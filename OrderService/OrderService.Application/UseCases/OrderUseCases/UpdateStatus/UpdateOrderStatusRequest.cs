using MediatR;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.OrderUseCases.UpdateStatus
{
    public record UpdateOrderStatusRequest(
        Guid OrderId,
        OrderStatus Status) : IRequest;
}
