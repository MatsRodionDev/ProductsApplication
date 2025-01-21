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

namespace OrderService.Application.UseCases.OrderUseCases.Create
{
    internal sealed class CreateOrderRequestHandler(
        IProductService productService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateOrderRequestHandler> logger) : IRequestHandler<CreateOrderRequest>
    {
        public async Task Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started handling CreateOrderRequest for UserId: {UserId}", request.UserId);

            var dto = mapper.Map<TakeProductDto>(request);

            var product = await productService.TakeProduct(dto, cancellationToken);

            if (product is null)
            {
                logger.LogWarning("Product with ProductId: {ProductId} could not be taken.", request.ProductId);
                throw new NotFoundException("Product not found or insufficient quantity.");
            }

            var orderId = Guid.NewGuid();
            var totalPrice = CalculateTotalPrice(product.Price, product.TakedQuantity);
            var order = CreateOrder(orderId, request.UserId, product, totalPrice);

            await unitOfWork.OrderRepository.CreateAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully created OrderId: {OrderId} for UserId: {UserId}, TotalPrice: {TotalPrice}",
                orderId, request.UserId, totalPrice);
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
