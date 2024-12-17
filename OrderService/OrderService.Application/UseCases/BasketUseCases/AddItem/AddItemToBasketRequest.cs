using MediatR;

namespace OrderService.Application.UseCases.BasketUseCases.AddItem
{
    public record AddItemToBasketRequest(
        Guid UserId,
        Guid ProductId,
        int Quantity) : IRequest;
}
