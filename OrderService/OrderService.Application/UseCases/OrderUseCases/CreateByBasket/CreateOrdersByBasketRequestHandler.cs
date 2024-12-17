using AutoMapper;
using MediatR;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.OrderUseCases.CreateByBasket
{
    internal sealed class CreateOrdersByBasketRequestHandler(
        IBasketRepository basketRepository,
        IProductService productService,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateOrdersByBasketRequest>
    {
        public async Task Handle(CreateOrdersByBasketRequest request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("There is no basket with such userId");

            if (!basket.BasketItems.Any())
            {
                throw new BadRequestException("There are no items in the basket");
            }

            foreach (var item in basket.BasketItems)
            {
                var product = productService.GetByIdAsync(item.ProductId);

                if (product == null || product.Quantity < item.Quantity)
                {
                    throw new BadRequestException($"Product {item.ProductId} is not available or insufficient quantity.");
                }

                if(product.UserId == request.UserId)
                {
                    throw new BadRequestException("You cannot place an order for your product");
                }

                var orderId = Guid.NewGuid();
                var totalPrice = product.Price * item.Quantity;

                var order = new Order
                {
                    Id = orderId,
                    BuyerId = request.UserId,
                    SellerId = product.UserId,
                    Quantity = item.Quantity,
                    ToTalPrice = totalPrice,
                    Status = OrderStatus.Processing.ToString(),
                    OrderItem = new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderId,
                        ProductId = product.Id,
                        ProdcutName = product.Name,
                        ProductPrice = product.Price
                    }
                };
                    
                await orderRepository.CreateAsync(order, cancellationToken);
            }

            foreach (var item in basket.BasketItems)
            {
                productService.UpdateQuantity(item.ProductId, item.Quantity);
            }

            basket.BasketItems.Clear();

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
