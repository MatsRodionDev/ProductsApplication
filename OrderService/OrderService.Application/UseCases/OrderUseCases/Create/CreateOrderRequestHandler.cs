using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Intefaces;
using OrderService.Application.Common.Models;
using OrderService.Domain.Enums;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.OrderUseCases.Create
{
    internal sealed class CreateOrderRequestHandler(
        IProductService productService,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CreateOrderRequest>
    {
        public async Task Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<TakeProductDto>(request);

            var product = productService.TakeProduct(dto);

            var orderId = Guid.NewGuid();
            var totalPrice = CalculateTotalPrice(product.Price, product.Quantity);

            var order = CreateOrder(orderId, request.UserId, product, totalPrice);

            await unitOfWork.OrderRepository.CreateAsync(order, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
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
