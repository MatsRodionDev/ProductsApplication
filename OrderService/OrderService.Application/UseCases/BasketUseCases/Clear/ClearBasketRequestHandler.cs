using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.BasketUseCases.Clear
{
    internal sealed class ClearBasketRequestHandler(
        IUnitOfWork unitOfWork,
        ILogger<ClearBasketRequestHandler> logger) : IRequestHandler<ClearBasketRequest>
    {
        public async Task Handle(ClearBasketRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started handling ClearBasketRequest for UserId: {UserId}", request.UserId);

            var basket = await unitOfWork.BasketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken);

            if (basket is null)
            {
                logger.LogWarning("No basket found for UserId: {UserId}", request.UserId);
                throw new NotFoundException("There is no basket with this userId");
            }

            logger.LogInformation("Basket found for UserId: {UserId}, proceeding to clear items.", request.UserId);

            basket.BasketItems.Clear();
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Basket successfully cleared for UserId: {UserId}", request.UserId);
        }
    }
}
