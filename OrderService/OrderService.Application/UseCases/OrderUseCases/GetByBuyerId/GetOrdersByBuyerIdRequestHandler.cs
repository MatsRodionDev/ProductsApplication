using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.OrderUseCases.GetByBuyerId
{
    internal sealed class GetOrdersByBuyerIdRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<GetOrdersByBuyerIdRequest, List<OrderResponseDto>>
    {
        public async Task<List<OrderResponseDto>> Handle(GetOrdersByBuyerIdRequest request, CancellationToken cancellationToken)
        {
            var orders = await unitOfWork.OrderRepository.GetByBuyerIdAsync(request.BuyerId, cancellationToken);

            return mapper.Map<List<OrderResponseDto>>(orders);
        }
    }
}
