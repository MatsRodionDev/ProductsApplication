using MediatR;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.OrderUseCases.UpdateStatus
{
    internal sealed class UpdateOrderStatusRequestHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrderStatusRequest>
    {
        public async Task Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
                ?? throw new NotFoundException("There is no order with this id");

            order.Status = request.Status.ToString();

            orderRepository.Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
