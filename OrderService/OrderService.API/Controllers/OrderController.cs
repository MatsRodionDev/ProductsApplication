using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Common.Dtos;
using OrderService.Application.UseCases.OrderUseCases.Cancel;
using OrderService.Application.UseCases.OrderUseCases.Create;
using OrderService.Application.UseCases.OrderUseCases.CreateByBasket;
using OrderService.Application.UseCases.OrderUseCases.GetByBuyerId;
using OrderService.Application.UseCases.OrderUseCases.GetByFilters;
using OrderService.Application.UseCases.OrderUseCases.GetBySellerId;
using OrderService.Application.UseCases.OrderUseCases.UpdateStatus;
using Shared.Consts;

namespace OrderService.API.Controllers
{
    [Authorize]
    [Controller]
    [Route("api/orders")]
    public class OrderController(
        IMediator mediator) : ControllerBase
    {
        [HttpGet("buyer")]
        public async Task<IActionResult> GetOrdersByBuyerId(CancellationToken cancellationToken)
        {
            var buyerId = Guid.Parse(User.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            var request = new GetOrdersByBuyerIdRequest(buyerId);

            var orders = await mediator.Send(request, cancellationToken );

            return Ok(orders);
        }

        [HttpGet("seller")]
        public async Task<IActionResult> GetOrdersBySellerId(CancellationToken cancellationToken)
        {
            var sellerId = Guid.Parse(User.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            var request = new GetOrdersBySellerIdRequest(sellerId);

            var orders = await mediator.Send(request, cancellationToken);

            return Ok(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersByFilters([FromQuery] GetOrdersByFiltersRequest request, CancellationToken cancellationToken)
        {
            var orders = await mediator.Send(request, cancellationToken);

            return Ok(orders);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateOrderStatusRequestDto dto, CancellationToken cancellationToken)
        {
            var request = new UpdateOrderStatusRequest(orderId, dto.Status);

            await mediator.Send(request, cancellationToken);    

            return NoContent();
        }

        [HttpPatch("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            var request = new CancelOrderRequest(orderId, userId);

            await mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto dto, CancellationToken cancellationToken = default)
        {
            var userId = Guid.Parse(User.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            var request = new CreateOrderRequest(userId, dto.ProductId, dto.Quantity);

            await mediator.Send(request, cancellationToken);

            return Created();
        }

        [HttpPost("basket")]
        public async Task<IActionResult> CreateOrderByBasket(CancellationToken cancellationToken = default)
        {
            var userId = Guid.Parse(User.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            var request = new CreateOrdersByBasketRequest(userId);

            await mediator.Send(request, cancellationToken);

            return Created();
        }
    }
}
