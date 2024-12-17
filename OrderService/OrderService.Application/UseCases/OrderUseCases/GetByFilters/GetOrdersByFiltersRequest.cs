using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Enums;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.OrderUseCases.GetByFilters
{
    public record GetOrdersByFiltersRequest(
        Guid? OrderId,
        OrderStatus? OrderStatus,
        int Page,
        int PageSize) : IRequest<List<OrderResponseDto>>;
}
