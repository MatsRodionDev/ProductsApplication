using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

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

            return new BasketResponseDto(
                basket.UserId,
                basket.BasketItems
                    .Select(item =>
                        GetBasketItemResponseDto(item.ProductId, item.Quantity))
                    .Where(dto => dto != null)
                    .ToList());
        }

        private BasketItemResponseDto? GetBasketItemResponseDto(Guid productId, int quantity)
        {
            var product = productService.GetByIdAsync(productId);

            if (product is null)
            {
                return null;
            }

            var totalPrice = product.Price * quantity;

            return new BasketItemResponseDto(
                product.Id,
                quantity,
                product.Price,
                totalPrice,
                product.Name,
                product.Description,
                product.Quantity);
        }
    }
}
