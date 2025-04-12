using MediatR;

namespace OrderService.Application.UseCases.OrderUseCases.Create
{
    public record CreateOrderRequest(
        Guid UserId,
        Guid ProductId,
        int Quantity) : IRequest;
}
