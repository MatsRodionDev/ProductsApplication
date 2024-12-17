using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.OrderUseCases.GetByBuyerId
{
    public record GetOrdersByBuyerIdRequest(
        Guid BuyerId) : IRequest<List<OrderResponseDto>>;
}
