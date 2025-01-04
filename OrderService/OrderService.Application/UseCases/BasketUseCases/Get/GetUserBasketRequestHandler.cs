using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using System.Threading;

namespace OrderService.Application.UseCases.BasketUseCases.Get
{
    internal sealed class GetUserBasketRequestHandler(
        IUnitOfWork unitOfWork,
        IProductService productService) : IRequestHandler<GetUserBasketRequest, BasketResponseDto>
    {
        public async Task<BasketResponseDto> Handle(GetUserBasketRequest request, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.BasketRepository.GetByUserIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("There is no basket with this id");

            var basketItemDtos = await Task.WhenAll(
                basket.BasketItems.Select(async item =>
                {
                    return await GetBasketItemResponseDto(item.ProductId, item.Quantity, cancellationToken);
                }));

            return new BasketResponseDto(
                basket.UserId,
                basketItemDtos.Where(i => i != null).ToList());     
        }

        private decimal CalculateTotalPrice(decimal price, int takedQuantity)
        {
            return price * takedQuantity;
        }

        private async Task<BasketItemResponseDto?> GetBasketItemResponseDto(Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            var product = await productService.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                return null;
            }

            var totalPrice = CalculateTotalPrice(product.Price, quantity);

            return new BasketItemResponseDto(
                product.Id,
                quantity,
                product.Price,
                totalPrice,
                product.Name,
                product.Quantity,
                product.Images
            );
        }
    }
}
