using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Common.Dtos;
using OrderService.Application.UseCases.BasketUseCases.AddItem;
using OrderService.Application.UseCases.BasketUseCases.Clear;
using OrderService.Application.UseCases.BasketUseCases.Create;
using OrderService.Application.UseCases.BasketUseCases.DeleteItem;
using OrderService.Application.UseCases.BasketUseCases.Get;
using OrderService.Application.UseCases.BasketUseCases.RemoveItem;

namespace OrderService.API.Controllers
{
    [Authorize]
    [Controller]
    [Route("api/baskets")]
    public class BasketController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasket(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var request = new GetUserBasketRequest(userId);

            var basket = await mediator.Send(request, cancellationToken);

            return Ok(basket);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasket([FromBody] CreateBasketRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);

            return Created();
        }

        [HttpPatch("item")]
        public async Task<IActionResult> AddItemToBasket([FromBody] AddItemToBasketRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPatch("item/{productId}")]
        public async Task<IActionResult> UpdateItemQuantityInBasket(Guid productId, [FromBody] UpdateItemQuantityRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var request = new UpdateItemQuantityInBasketRequest(userId, productId, dto.Quantity);

            await mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPatch("clear")]
        public async Task<IActionResult> ClearBasket([FromBody] Guid userId, CancellationToken cancellationToken)
        {
            var request = new ClearBasketRequest(userId);

            await mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpDelete("item/{productId}")]
        public async Task<IActionResult> DeleteItemFromBasket(Guid productId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var request = new DeleteItemFromBasketRequest(userId, productId);

            await mediator.Send(request, cancellationToken);

            return NoContent();
        }
    }
}
