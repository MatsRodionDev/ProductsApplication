using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
        IMapper mapper,
        ILogger<CreateOrdersByBasketRequestHandler> logger) : IRequestHandler<CreateOrdersByBasketRequest>
    {
        public async Task Handle(CreateOrdersByBasketRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started handling CreateOrdersByBasketRequest for UserId: {UserId}", request.UserId);

            var basket = await unitOfWork.BasketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken);

            if (basket is null)
            {
                logger.LogWarning("Basket not found for UserId: {UserId}", request.UserId);
                throw new NotFoundException("There is no basket with such userId");
            }

            if (!basket.BasketItems.Any())
            {
                logger.LogWarning("Basket for UserId: {UserId} is empty.", request.UserId);
                throw new BadRequestException("There are no items in the basket");
            }

            var productsToTake = mapper.Map<List<TakeProductDto>>(basket.BasketItems);

            var takedProducts = await productService.TakeProducts(productsToTake, cancellationToken);

            foreach (var product in takedProducts)
            {
                await CreateOrdersAsync(product, request.UserId, cancellationToken);
            }

            basket.BasketItems.Clear();
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully created orders from basket for UserId: {UserId}", request.UserId);
        }

        private async Task CreateOrdersAsync(TakedProduct product, Guid buyerId, CancellationToken cancellationToken = default)
        {
            var orderId = Guid.NewGuid();
            var totalPrice = CalculateTotalPrice(product.Price, product.TakedQuantity);
            var order = CreateOrder(orderId, buyerId, product, totalPrice);

            await unitOfWork.OrderRepository.CreateAsync(order, cancellationToken);
        }

        private decimal CalculateTotalPrice(decimal price, int takedQuantity)
        {
            return price * takedQuantity;
        }

        private Order CreateOrder(Guid orderId, Guid buyerId, TakedProduct product, decimal totalPrice)
        {
            return new Order
            {
                Id = orderId,
                BuyerId = buyerId,
                SellerId = product.UserId,
                Quantity = product.TakedQuantity,
                ToTalPrice = totalPrice,
                Status = OrderStatus.Processing.ToString(),
                OrderItem = CreateOrderItem(orderId, product)
            };
        }

        private OrderItem CreateOrderItem(Guid orderId, TakedProduct product)
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
