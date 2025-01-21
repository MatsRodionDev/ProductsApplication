using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.OrderUseCases.UpdateStatus
{
    internal sealed class UpdateOrderStatusRequestHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateOrderStatusRequestHandler> logger) : IRequestHandler<UpdateOrderStatusRequest>
    {
        public async Task Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started handling UpdateOrderStatusRequest for OrderId: {OrderId}, New Status: {Status}",
                request.OrderId, request.Status);

            var order = await unitOfWork.OrderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                logger.LogWarning("Order not found for OrderId: {OrderId}", request.OrderId);
                throw new NotFoundException("There is no order with this id");
            }

            order.Status = request.Status.ToString();

            unitOfWork.OrderRepository.Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully updated status for OrderId: {OrderId} to {Status}",
                request.OrderId, request.Status);
        }
    }
}
