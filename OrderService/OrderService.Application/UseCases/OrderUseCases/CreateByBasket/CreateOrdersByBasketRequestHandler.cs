﻿using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Intefaces;
using OrderService.Application.Common.Models;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.OrderUseCases.CreateByBasket
{
    internal sealed class CreateOrdersByBasketRequestHandler(
        IProductService productService,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CreateOrdersByBasketRequest>
    {
        public async Task Handle(CreateOrdersByBasketRequest request, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.BasketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("There is no basket with such userId");

            if (!basket.BasketItems.Any())
            {
                throw new BadRequestException("There are no items in the basket");
            }

            var productsToTake = mapper.Map<List<TakeProductDto>>(basket.BasketItems);

            var takedProducts = productService.TakeProducts(productsToTake);

            foreach(var product in takedProducts) 
            {
                await CreateOrdersAsync(product, request.UserId, cancellationToken);
            }

            basket.BasketItems.Clear();

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task CreateOrdersAsync(Product product, Guid buyerId, CancellationToken cancellationToken = default)
        {
            var orderId = Guid.NewGuid();
            var totalPrice = CalculateTotalPrice(product.Price, product.Quantity);
            var order = CreateOrder(orderId, buyerId, product, totalPrice);

            await unitOfWork.OrderRepository.CreateAsync(order, cancellationToken);
        }

        private decimal CalculateTotalPrice(decimal price, int quantity)
        {
            return price * quantity;
        }

        private Order CreateOrder(Guid orderId, Guid buyerId, Product product, decimal totalPrice)
        {
            return new Order
            {
                Id = orderId,
                BuyerId = buyerId,
                SellerId = product.UserId,
                Quantity = product.Quantity,
                ToTalPrice = totalPrice,
                Status = OrderStatus.Processing.ToString(),
                OrderItem = CreateOrderItem(orderId, product)
            };
        }

        private OrderItem CreateOrderItem(Guid orderId, Product product)
        {
            return new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = product.Id,
                ProdcutName = product.Name,
                ProductPrice = product.Price
            };
        }
    }
}
