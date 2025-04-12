using MediatR;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.OrderUseCases.Cancel
{
    internal sealed class CancelOrderRequestHandler(
        IUnitOfWork unitOfWork) : IRequestHandler<CancelOrderRequest>
    {
        public async Task Handle(CancelOrderRequest request, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.OrderRepository.GetByIdAsync(request.OrderId, cancellationToken)
                ?? throw new NotFoundException("There is no order with this id");

            if(order.SellerId != request.UserId && order.BuyerId != request.UserId)
            {
                throw new UnauthorizedException("You cannot cncel this order");
            }

            if(order.Status == OrderStatus.Canceled.ToString() || order.Status == OrderStatus.Returned.ToString())
            {
                throw new BadRequestException("This order is already canceled");
            }

            order.Status = OrderStatus.Canceled.ToString();

            unitOfWork.OrderRepository.Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
