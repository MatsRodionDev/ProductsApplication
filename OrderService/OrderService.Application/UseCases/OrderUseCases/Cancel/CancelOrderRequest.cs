using MediatR;

namespace OrderService.Application.UseCases.OrderUseCases.Cancel
{
    public record CancelOrderRequest(
        Guid OrderId,
        Guid UserId) : IRequest;
}
