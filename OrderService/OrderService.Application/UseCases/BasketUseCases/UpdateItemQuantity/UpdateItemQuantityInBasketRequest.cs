using MediatR;

namespace OrderService.Application.UseCases.BasketUseCases.RemoveItem
{
    public record UpdateItemQuantityInBasketRequest(
        Guid UserId, 
        Guid ProductId,
        int Quantity) : IRequest;
}
