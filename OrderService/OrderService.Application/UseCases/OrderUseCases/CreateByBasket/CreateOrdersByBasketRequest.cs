using MediatR;

namespace OrderService.Application.UseCases.OrderUseCases.CreateByBasket
{
    public record CreateOrdersByBasketRequest(Guid UserId) : IRequest;
}
