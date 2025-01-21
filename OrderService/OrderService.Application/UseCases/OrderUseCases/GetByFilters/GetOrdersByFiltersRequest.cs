using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.OrderUseCases.GetByFilters
{
    public record GetOrdersByFiltersRequest(
        string? ProductName,
        OrderStatus? OrderStatus,
        int Page = 1,
        int PageSize = 5) : IRequest<List<OrderResponseDto>>;
}
