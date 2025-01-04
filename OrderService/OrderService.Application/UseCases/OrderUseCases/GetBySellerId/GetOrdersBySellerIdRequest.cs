using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.OrderUseCases.GetBySellerId
{
    public record GetOrdersBySellerIdRequest(
        Guid SellerId) : IRequest<List<OrderResponseDto>>;
}
