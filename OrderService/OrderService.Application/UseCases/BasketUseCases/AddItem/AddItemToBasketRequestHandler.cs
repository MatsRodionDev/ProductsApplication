using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.BasketUseCases.AddItem
{
    internal sealed class AddItemToBasketRequestHandler(
        IUnitOfWork unitOfWork,
        IProductService productService,
        ILogger<AddItemToBasketRequestHandler> logger) : IRequestHandler<AddItemToBasketRequest>
    {
        public async Task Handle(AddItemToBasketRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling AddItemToBasketRequest for UserId: {UserId}, ProductId: {ProductId}, Quantity: {Quantity}",
                request.UserId, request.ProductId, request.Quantity);

            var basket = await unitOfWork.BasketRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (basket is null)
            {
                logger.LogWarning("Basket not found for UserId: {UserId}", request.UserId);
                throw new NotFoundException("This basket doesn't exist");
            }

            var product = await productService.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                logger.LogWarning("Product not found with ProductId: {ProductId}", request.ProductId);
                throw new NotFoundException("This product doesn't exist");
            }

            if (product.Quantity < request.Quantity)
            {
                logger.LogWarning("Insufficient product quantity for ProductId: {ProductId}. Requested: {Requested}, Available: {Available}",
                    request.ProductId, request.Quantity, product.Quantity);
                throw new BadRequestException("There are no products with this id in this quantity");
            }

            if (product.UserId == request.UserId)
            {
                logger.LogWarning("User tried to add their own product to basket. UserId: {UserId}, ProductId: {ProductId}",
                    request.UserId, request.ProductId);
                throw new BadRequestException("You cannot add your product in your basket");
            }

            var item = basket.BasketItems.Find(i => i.ProductId == request.ProductId);

            if (item is not null)
            {
                logger.LogWarning("Item already exists in the basket. UserId: {UserId}, ProductId: {ProductId}",
                    request.UserId, request.ProductId);
                throw new BadRequestException("This item is already in the basket");
            }

            var basketItem = new BasketItem(basket.Id, request.ProductId, request.Quantity);
            await unitOfWork.BasketItemRepository.CreateAsync(basketItem, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Item successfully added to basket. UserId: {UserId}, ProductId: {ProductId}, Quantity: {Quantity}",
                request.UserId, request.ProductId, request.Quantity);
        }
    }
}
