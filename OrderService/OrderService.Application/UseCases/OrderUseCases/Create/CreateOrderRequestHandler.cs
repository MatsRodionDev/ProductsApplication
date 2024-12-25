using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Intefaces;
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
            var totalPrice = product.Price * product.Quantity;

            var order = new Order
            {
                Id = orderId,
                BuyerId = request.UserId,
                SellerId = product.UserId,
                Quantity = product.Quantity,
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

            await unitOfWork.OrderRepository.CreateAsync(order, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
