using MediatR;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.OrderUseCases.Create
{
    internal sealed class CreateOrderRequestHandler(
        IOrderRepository orderRepository,
        IProductService productService,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderRequest>
    {
        public async Task Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var product = productService.GetByIdAsync(request.ProductId)
                ?? throw new NotFoundException($"There is not product {request.ProductId}");

            if (product.Quantity < request.Quantity)
            {
                throw new BadRequestException($"Product {request.ProductId} insufficient quantity.");
            }

            if (product.UserId == request.UserId)
            {
                throw new BadRequestException("You cannot place an order for your product");
            }

            var orderId = Guid.NewGuid();
            var totalPrice = product.Price * request.Quantity;

            var order = new Order
            {
                Id = orderId,
                BuyerId = request.UserId,
                SellerId = product.UserId,
                Quantity = request.Quantity,
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

            productService.UpdateQuantity(request.ProductId, request.Quantity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
