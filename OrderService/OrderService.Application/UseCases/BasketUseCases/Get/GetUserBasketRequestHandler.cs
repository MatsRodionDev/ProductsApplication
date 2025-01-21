using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using System.Threading;

namespace OrderService.Application.UseCases.BasketUseCases.Get
{
    internal sealed class GetUserBasketRequestHandler(
        IUnitOfWork unitOfWork,
        IProductService productService,
        ILogger<GetUserBasketRequestHandler> logger) : IRequestHandler<GetUserBasketRequest, BasketResponseDto>
    {
        public async Task<BasketResponseDto> Handle(GetUserBasketRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started handling GetUserBasketRequest for UserId: {UserId}", request.UserId);

            var basket = await unitOfWork.BasketRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (basket is null)
            {
                logger.LogError("Basket not found for UserId: {UserId}.", request.UserId);
                throw new NotFoundException("There is no basket with this id");
            }

            var basketItemDtos = await Task.WhenAll(
                basket.BasketItems.Select(item =>
                    GetBasketItemResponseDto(item.ProductId, item.Quantity, cancellationToken)));

            var validItems = basketItemDtos.Where(i => i != null).ToList();

            logger.LogInformation("Successfully retrieved basket for UserId: {UserId}. Item count: {ItemCount}", request.UserId, validItems.Count);

            return new BasketResponseDto(
                basket.UserId,
                validItems);
        }

        private async Task<BasketItemResponseDto?> GetBasketItemResponseDto(Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            var product = await productService.GetByIdAsync(productId, cancellationToken);

            if (product is null)
            {
                logger.LogWarning("Product not found for ProductId: {ProductId}", productId);
                return null;
            }

            return new BasketItemResponseDto(
                product.Id,
                quantity,
                product.Price,
                product.Price * quantity,
                product.Name,
                product.Quantity,
                product.Images
            );
        }
    }
}
