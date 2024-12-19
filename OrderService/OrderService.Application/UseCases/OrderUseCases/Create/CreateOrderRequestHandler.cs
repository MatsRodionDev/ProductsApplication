using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
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
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CreateOrderRequest>
    {
        public async Task Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var product = productService.GetByIdAsync(request.ProductId)
                ?? throw new NotFoundException($"There is no product {request.ProductId}");

            if (product.Quantity < request.Quantity)
            {
                throw new BadRequestException($"Insufficient quantity of product {request.ProductId}");
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

            var dto = mapper.Map<TakeProductDto>(order);

            productService.UpdateQuantity(dto);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
