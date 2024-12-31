using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.BasketUseCases.Get
{
    internal sealed class GetUserBasketRequestHandler(
        IBasketRepository basketRepository,
        IProductService productService) : IRequestHandler<GetUserBasketRequest, BasketResponseDto>
    {
        public async Task<BasketResponseDto> Handle(GetUserBasketRequest request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetByUserIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("There is no basket with this id");

            var basketItemDtos = await Task.WhenAll(
                basket.BasketItems.Select(async item =>
                {
                    var product = await productService.GetByIdAsync(item.ProductId, cancellationToken);

                    if (product == null)
                    {
                        return null;
                    }

                    var totalPrice = CalculateTotalPrice(product.Price, item.Quantity);

                    return new BasketItemResponseDto(
                        product.Id,
                        item.Quantity,
                        product.Price,
                        totalPrice,
                        product.Name,
                        product.Quantity,
                        product.Images
                    );
                }));

            return new BasketResponseDto(
                basket.UserId,
                basketItemDtos.Where(i => i != null).ToList());     
        }

        private decimal CalculateTotalPrice(decimal price, int takedQuantity)
        {
            return price * takedQuantity;
        }
    }
}
