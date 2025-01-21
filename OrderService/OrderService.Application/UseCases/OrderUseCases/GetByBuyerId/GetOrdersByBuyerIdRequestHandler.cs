using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Exceptions;

namespace OrderService.Application.UseCases.OrderUseCases.GetByBuyerId
{
    internal sealed class GetOrdersByBuyerIdRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetOrdersByBuyerIdRequestHandler> logger) : IRequestHandler<GetOrdersByBuyerIdRequest, List<OrderResponseDto>>
    {
        public async Task<List<OrderResponseDto>> Handle(GetOrdersByBuyerIdRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started handling GetOrdersByBuyerIdRequest for BuyerId: {BuyerId}", request.BuyerId);

            var orders = await unitOfWork.OrderRepository.GetByBuyerIdAsync(request.BuyerId, cancellationToken);

            logger.LogInformation("Successfully found orders for BuyerId: {BuyerId}", request.BuyerId);

            return mapper.Map<List<OrderResponseDto>>(orders);
        }
    }
}
