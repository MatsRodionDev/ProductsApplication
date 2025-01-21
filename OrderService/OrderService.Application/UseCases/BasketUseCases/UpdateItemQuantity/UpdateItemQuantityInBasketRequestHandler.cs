using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.BasketUseCases.RemoveItem
{
    internal sealed class UpdateItemQuantityInBasketRequestHandler(
        IUnitOfWork unitOfWork,
        IProductService productService,
        ILogger<UpdateItemQuantityInBasketRequestHandler> logger) : IRequestHandler<UpdateItemQuantityInBasketRequest>
    {
        public async Task Handle(UpdateItemQuantityInBasketRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling UpdateItemQuantityInBasketRequest. UserId: {UserId}, ProductId: {ProductId}, Requested Quantity: {Quantity}",
                request.UserId, request.ProductId, request.Quantity);

            var basket = await unitOfWork.BasketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken);

            if (basket is null)
            {
                logger.LogError("Basket not found for UserId: {UserId}.", request.UserId);
                throw new NotFoundException("No basket found for the provided userId.");
            }

            var product = await productService.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                logger.LogError("Product not found for ProductId: {ProductId}.", request.ProductId);
                throw new NotFoundException("No product found with the provided productId.");
            }

            var item = basket.BasketItems.Find(i => i.ProductId == request.ProductId);

            if (item is null)
            {
                logger.LogError("Product with ProductId: {ProductId} not found in basket for UserId: {UserId}.", request.ProductId, request.UserId);
                throw new NotFoundException("Product not found in the basket.");
            }

            if (product.Quantity < request.Quantity)
            {
                logger.LogError("Insufficient product quantity for ProductId: {ProductId}. Requested Quantity: {RequestedQuantity}, Available Quantity: {AvailableQuantity}.",
                    request.ProductId, request.Quantity, product.Quantity);
                throw new BadRequestException("Not enough quantity of this product available.");
            }

            item.Quantity = request.Quantity;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully updated item quantity in basket for UserId: {UserId}, ProductId: {ProductId}, New Quantity: {NewQuantity}.",
                request.UserId, request.ProductId, request.Quantity);
        }
    }
}
