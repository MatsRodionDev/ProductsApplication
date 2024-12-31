using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.OrderUseCases.GetBySellerId
{
    internal sealed class GetOrdersBySellerIdRequestHandler(
        IOrderRepository orderRepository,
        IMapper mapper) : IRequestHandler<GetOrdersBySellerIdRequest, List<OrderResponseDto>>
    {
        public async Task<List<OrderResponseDto>> Handle(GetOrdersBySellerIdRequest request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetBySellerIdAsync(request.SellerId, cancellationToken);

            return mapper.Map<List<OrderResponseDto>>(orders);
        }
    }
}
