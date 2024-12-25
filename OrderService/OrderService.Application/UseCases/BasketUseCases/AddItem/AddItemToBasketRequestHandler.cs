using MediatR;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.BasketUseCases.AddItem
{
    internal sealed class AddItemToBasketRequestHandler(
        IUnitOfWork unitOfWork,
        IProductService productService) : IRequestHandler<AddItemToBasketRequest>
    {
        public async Task Handle(AddItemToBasketRequest request, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.BasketRepository.GetByUserIdAsync(request.UserId, cancellationToken)
                ?? throw new Exception("This basket doesnt exist");

            var product = productService.GetByIdAsync(request.ProductId)
                ?? throw new Exception("This product doesnt exist");

            if (product.Quantity < request.Quantity)
            {
                throw new BadRequestException("There are no products with this id in this quantity");
            }

            if (product.UserId == request.UserId)
            {
                throw new BadRequestException("You cannot add your product in your basket");
            }

            var item = basket.BasketItems
                .Find(i => i.ProductId == request.ProductId);

            if (item is not null)
            {
                throw new Exception("This item is already in the basket");
            }

            await unitOfWork.BasketItemRepository.CreateAsync(new BasketItem(basket.Id, request.ProductId, request.Quantity), cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
