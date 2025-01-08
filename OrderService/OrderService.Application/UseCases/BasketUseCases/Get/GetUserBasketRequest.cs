using MediatR;
using OrderService.Application.Common.Dtos;

namespace OrderService.Application.UseCases.BasketUseCases.Get
{
    public record GetUserBasketRequest(Guid UserId) : IRequest<BasketResponseDto>;
}
