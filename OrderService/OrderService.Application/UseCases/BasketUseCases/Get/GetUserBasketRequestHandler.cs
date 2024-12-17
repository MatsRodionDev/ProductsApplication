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

            return new BasketResponseDto(
                basket.UserId,
                basket.BasketItems
                    .Select(item =>
                    {
                        var product = productService.GetByIdAsync(item.ProductId);

                        if (product is null)
                        {
                            return null;
                        }

                        var totalPrice = product.Price * item.Quantity;

                        return new BasketItemResponseDto(
                            product.Id,
                            item.Quantity,
                            product.Price,
                            totalPrice,
                            product.Name,
                            product.Description,
                            product.Quantity);
                    })
                    .Where(dto => dto != null)
                    .ToList());
        }
    }
}
