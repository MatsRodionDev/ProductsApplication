using MediatR;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.BasketUseCases.Clear
{
    internal sealed class ClearBasketRequestHandler(
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ClearBasketRequest>
    {
        public async Task Handle(ClearBasketRequest request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken)
                ?? throw new Exception("");

            basket.BasketItems.Clear();
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
