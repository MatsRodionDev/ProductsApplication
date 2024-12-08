using MediatR;
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
    [Route("[controller]")]
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

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] FiltersRequestDto dto, CancellationToken cancellationToken)
        {
            var request = new GetProductsByUserIdRequest(
                id,
                dto.Category.ToString(),
                dto.Name,
                dto.OrderBy, 
                dto.Asc,
                dto.Page,
                dto.PageSize);

            var product = await mediator.Send(request, cancellationToken);

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);

            return Created();        
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequestDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateProductCommand(
                id,
                dto.Name,
                dto.Description,
                dto.Quantity,
                dto.Price,
                dto.UserId);

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id,[FromBody] Guid userId, CancellationToken cancellationToken)
        {
            var command = new DeleteProductCommand(id, userId);

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("{id}/image")]
        public async Task<IActionResult> CreateImageToProduct(Guid id, [FromForm] CreateImageToProductRequestDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateImageToProductCommand(
                id,
                dto.UserId,
                dto.File);

            await mediator.Send(command, cancellationToken);

            return Created();
        }

        [HttpDelete("{productId}/image/{imageId}")]
        public async Task<IActionResult> DeleteImageToProduct(Guid productId, Guid imageId, [FromBody] Guid userId, CancellationToken cancellationToken)
        {
            var command = new DeleteImageToProductCommand(productId, imageId, userId);

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("{id}/category")]
        public async Task<IActionResult> CreateCategoryToProduct(Guid id, [FromBody] CreateCategoryToProductRequestDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateCategoryToProductCommand(
                id,
                dto.UserId,
                dto.Category.ToString());

            await mediator.Send(command, cancellationToken);

            return Created();
        }

        [HttpDelete("{productId}/category/{categoryId}")]
        public async Task<IActionResult> DeleteCategoryToProduct(Guid productId, Guid categoryId, [FromBody] Guid userId, CancellationToken cancellationToken)
        {
            var command = new DeleteCategoryToProductCommand(productId, categoryId, userId);

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
