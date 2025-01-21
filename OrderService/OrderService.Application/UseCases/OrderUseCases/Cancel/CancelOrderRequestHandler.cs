using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.OrderUseCases.Cancel
{
    internal sealed class CancelOrderRequestHandler(
        IUnitOfWork unitOfWork,
        ILogger<CancelOrderRequestHandler> logger) : IRequestHandler<CancelOrderRequest>
    {
        public async Task Handle(CancelOrderRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling CancelOrderRequest for OrderId: {OrderId}, UserId: {UserId}",
                request.OrderId, request.UserId);

            var order = await unitOfWork.OrderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                logger.LogError("Order not found for OrderId: {OrderId}.", request.OrderId);
                throw new NotFoundException("There is no order with this id");
            }

            if (order.SellerId != request.UserId && order.BuyerId != request.UserId)
            {
                logger.LogError("User {UserId} attempted to cancel an order they are not involved in. OrderId: {OrderId}.",
                    request.UserId, request.OrderId);
                throw new UnauthorizedException("You cannot cancel this order");
            }

            if (order.Status == OrderStatus.Canceled.ToString() || order.Status == OrderStatus.Returned.ToString())
            {
                logger.LogError("Order {OrderId} is already canceled or returned. Current Status: {Status}.",
                    request.OrderId, order.Status);
                throw new BadRequestException("This order is already canceled");
            }

            order.Status = OrderStatus.Canceled.ToString();
            unitOfWork.OrderRepository.Update(order);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Successfully canceled order with OrderId: {OrderId}. Changes saved.", request.OrderId);
        }
    }
}
