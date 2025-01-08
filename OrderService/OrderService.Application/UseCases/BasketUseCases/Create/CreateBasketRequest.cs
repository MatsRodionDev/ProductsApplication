using MediatR;

namespace OrderService.Application.UseCases.BasketUseCases.Create
{
    public record CreateBasketRequest(
        Guid UserId) : IRequest;
}
