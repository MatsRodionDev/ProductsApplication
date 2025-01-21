using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Filters;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Exceptions;

namespace OrderService.Application.UseCases.OrderUseCases.GetByFilters
{
    internal sealed class GetOrdersByFiltersRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetOrdersByFiltersRequestHandler> logger) : IRequestHandler<GetOrdersByFiltersRequest, List<OrderResponseDto>>
    {
        public async Task<List<OrderResponseDto>> Handle(GetOrdersByFiltersRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started handling GetOrdersByFiltersRequest with filters");

            var filters = mapper.Map<OrderFilters>(request);

            var orders = await unitOfWork.OrderRepository.GetOrdersByFiltersAsync(filters, cancellationToken);

            logger.LogInformation("Successfully found orders with provided filters: {Filters}", filters);

            return mapper.Map<List<OrderResponseDto>>(orders);
        }
    }
}
