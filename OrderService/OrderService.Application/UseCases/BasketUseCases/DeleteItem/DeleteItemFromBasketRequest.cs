using MediatR;

namespace OrderService.Application.UseCases.BasketUseCases.DeleteItem
{
    public record DeleteItemFromBasketRequest(
        Guid UserId, 
        Guid ProductId) : IRequest;
}
