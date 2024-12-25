﻿using MediatR;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.BasketUseCases.RemoveItem
{
    internal sealed class UpdateItemQuantityInBasketRequestHandler(
        IUnitOfWork unitOfWork,
        IProductService productService) : IRequestHandler<UpdateItemQuantityInBasketRequest>
    {
        public async Task Handle(UpdateItemQuantityInBasketRequest request, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.BasketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("There is no basket with such userId.");

            var product = productService.GetByIdAsync(request.ProductId)
                ?? throw new NotFoundException("There is no prdocut with this id.");

            var item = basket.BasketItems.Find(i => i.ProductId == request.ProductId)
               ?? throw new NotFoundException("There is no product with this ID in the basket");

            if (product.Quantity < request.Quantity)
            {
                throw new BadRequestException("There are no products with this id in this quantity");
            }

            item.Quantity = request.Quantity;

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
