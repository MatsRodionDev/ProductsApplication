using MediatR;

namespace OrderService.Application.UseCases.BasketUseCases.Clear
{
    public record ClearBasketRequest(Guid UserId) : IRequest;
}
