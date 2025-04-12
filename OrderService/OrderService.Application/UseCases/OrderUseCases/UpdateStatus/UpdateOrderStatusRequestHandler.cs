using MediatR;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.OrderUseCases.UpdateStatus
{
    internal sealed class UpdateOrderStatusRequestHandler(
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrderStatusRequest>
    {
        public async Task Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.OrderRepository.GetByIdAsync(request.OrderId, cancellationToken)
                ?? throw new NotFoundException("There is no order with this id");

            order.Status = request.Status.ToString();

            unitOfWork.OrderRepository.Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
