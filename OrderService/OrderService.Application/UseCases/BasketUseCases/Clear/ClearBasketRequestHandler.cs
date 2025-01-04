using MediatR;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.BasketUseCases.Clear
{
    internal sealed class ClearBasketRequestHandler(
        IUnitOfWork unitOfWork) : IRequestHandler<ClearBasketRequest>
    {
        public async Task Handle(ClearBasketRequest request, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.BasketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("There is no basket with this userId");

            basket.BasketItems.Clear();
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
