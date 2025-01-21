using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.BasketUseCases.DeleteItem
{
    internal sealed class DeleteItemFromBasketRequestHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteItemFromBasketRequestHandler> logger) : IRequestHandler<DeleteItemFromBasketRequest>
    {
        public async Task Handle(DeleteItemFromBasketRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started handling DeleteItemFromBasketRequest for UserId: {UserId}, ProductId: {ProductId}",
                request.UserId, request.ProductId);

            var basket = await unitOfWork.BasketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken);

            if (basket is null)
            {
                logger.LogError("Basket not found for UserId: {UserId}", request.UserId);
                throw new NotFoundException("There is no basket with this id");
            }

            var item = basket.BasketItems
                .Find(i => i.ProductId == request.ProductId);

            if (item is not null)
            {
                logger.LogInformation("Item found in basket, removing ItemId: {ProductId}", request.ProductId);
                basket.BasketItems.Remove(item);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Basket updated successfully after removing item for UserId: {UserId}, ProductId: {ProductId}",
                request.UserId, request.ProductId);
        }
    }
}
