using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.Common.Dto.Requests;
using ProductsService.Application.UseCases.ProductUseCases.Commands.Create;
using ProductsService.Application.UseCases.ProductUseCases.Commands.CreateCategory;
using ProductsService.Application.UseCases.ProductUseCases.Commands.CreateImage;
using ProductsService.Application.UseCases.ProductUseCases.Commands.Delete;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteCategory;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteImage;
using ProductsService.Application.UseCases.ProductUseCases.Commands.Update;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByFilters;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetById;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByUserId;

namespace ProductsService.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController(
        IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetProductsByFiltersRequest request, CancellationToken cancellationToken)
        {
            var products = await mediator.Send(request, cancellationToken);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var request = new GetProductByIdRequest(id);

            var product = await mediator.Send(request, cancellationToken);

            return Ok(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var command = new CreateProductCommand(
                dto.Name,
                dto.Description,
                dto.Quantity,
                dto.Price,
                userId,
                dto.ImageFiles,
                dto.Categories);

            await mediator.Send(command, cancellationToken);

            return Created();        
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var command = new UpdateProductCommand(
                id,
                dto.Name,
                dto.Description,
                dto.Quantity,
                dto.Price,
                userId);

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var command = new DeleteProductCommand(id, userId);

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id}/images")]
        public async Task<IActionResult> CreateImageToProduct(Guid id, [FromForm] CreateImageToProductRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var command = new CreateImageToProductCommand(
                id,
                userId,
                dto.File);

            await mediator.Send(command, cancellationToken);

            return Created();
        }

        [Authorize]
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImageToProduct(Guid productId, Guid imageId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var command = new DeleteImageToProductCommand(productId, imageId, userId);

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id}/categories")]
        public async Task<IActionResult> CreateCategoryToProduct(Guid id, [FromBody] CreateCategoryToProductRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var command = new CreateCategoryToProductCommand(
                id,
                userId,
                dto.Category.ToString());

            await mediator.Send(command, cancellationToken);

            return Created();
        }

        [Authorize]
        [HttpDelete("{productId}/categories/{categoryId}")]
        public async Task<IActionResult> DeleteCategoryToProduct(Guid productId, Guid categoryId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            var command = new DeleteCategoryToProductCommand(productId, categoryId, userId);

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
