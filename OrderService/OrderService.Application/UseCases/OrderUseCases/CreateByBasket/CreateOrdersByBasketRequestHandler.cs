using AutoMapper;
using MediatR;
using OrderService.Application.Common.Dtos;
using OrderService.Application.Common.Intefaces;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using System.Text;

namespace OrderService.Application.UseCases.OrderUseCases.CreateByBasket
{
    internal sealed class CreateOrdersByBasketRequestHandler(
        IBasketRepository basketRepository,
        IProductService productService,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CreateOrdersByBasketRequest>
    {
        public async Task Handle(CreateOrdersByBasketRequest request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("There is no basket with such userId");

            if (!basket.BasketItems.Any())
            {
                throw new BadRequestException("There are no items in the basket");
            }

            List<TakeProductDto> productsToTake = [];

            foreach (var item in basket.BasketItems)
            {
                var product = productService.GetByIdAsync(item.ProductId)
                    ?? throw new NotFoundException($"There is no product {item.ProductId}");

                if (product.Quantity < item.Quantity)
                {
                    throw new BadRequestException($"Insufficient quantity of product {product.Id}");
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

                productsToTake.Add(mapper.Map<TakeProductDto>(order));
            }

            List<ReturnProductDto> productsToReturn = [];

            var errors = new StringBuilder();

            foreach (var dto in productsToTake)
            {
                try
                {
                    productService.UpdateQuantity(dto);
                } 
                catch(BadRequestException ex)
                {
                    errors.Append(ex.Message);
                    productsToReturn.Add(mapper.Map<ReturnProductDto>(dto));
                }
                catch(NotFoundException ex)
                {
                    errors.Append(ex.Message);
                }
            }

            if (productsToReturn.Any() || errors.Length > 0)
            {
                foreach(var dto in productsToReturn)
                {
                    productService.ReturnProduct(dto);
                }

                throw new BadRequestException(errors.ToString());
            }

            basket.BasketItems.Clear();

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
