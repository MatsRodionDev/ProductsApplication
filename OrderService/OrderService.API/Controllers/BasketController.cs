﻿using MediatR;
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
        public async Task<IActionResult> CreateBasket(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var request = new CreateBasketRequest(userId);

            await mediator.Send(request, cancellationToken);

            return Created();
        }

        [HttpPatch("items")]
        public async Task<IActionResult> AddItemToBasket([FromBody] AddItemToBasketRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var request = new AddItemToBasketRequest(userId, dto.ProductId, dto.Quantity);

            await mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPatch("items/{productId}")]
        public async Task<IActionResult> UpdateItemQuantityInBasket(Guid productId, [FromBody] UpdateItemQuantityRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var request = new UpdateItemQuantityInBasketRequest(userId, productId, dto.Quantity);

            await mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearBasket(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var request = new ClearBasketRequest(userId);

            await mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> DeleteItemFromBasket(Guid productId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var request = new DeleteItemFromBasketRequest(userId, productId);

            await mediator.Send(request, cancellationToken);

            return NoContent();
        }
    }
}
