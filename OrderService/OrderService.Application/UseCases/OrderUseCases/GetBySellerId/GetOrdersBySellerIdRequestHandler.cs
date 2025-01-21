using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Exceptions;

namespace OrderService.Application.UseCases.OrderUseCases.GetBySellerId
{
    internal sealed class GetOrdersBySellerIdRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetOrdersBySellerIdRequestHandler> logger) : IRequestHandler<GetOrdersBySellerIdRequest, List<OrderResponseDto>>
    {
        public async Task<List<OrderResponseDto>> Handle(GetOrdersBySellerIdRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started handling GetOrdersBySellerIdRequest for SellerId: {SellerId}", request.SellerId);

            var orders = await unitOfWork.OrderRepository.GetBySellerIdAsync(request.SellerId, cancellationToken);

            logger.LogInformation("Successfully retrieved orders for SellerId: {SellerId}", request.SellerId);

            return mapper.Map<List<OrderResponseDto>>(orders);
        }
    }
}
