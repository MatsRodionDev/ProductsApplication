using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Filters;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.OrderUseCases.GetByFilters
{
    internal sealed class GetOrdersByFiltersRequestHandler(
        IOrderRepository orderRepository,
        IMapper mapper) : IRequestHandler<GetOrdersByFiltersRequest, List<OrderResponseDto>>
    {
        public async Task<List<OrderResponseDto>> Handle(GetOrdersByFiltersRequest request, CancellationToken cancellationToken)
        {
            var filters = mapper.Map<OrderFilters>(request);

            var orders = await orderRepository.GetOrdersByFiltersAsync(filters, cancellationToken);

            return mapper.Map<List<OrderResponseDto>>(orders);
        }
    }
}
