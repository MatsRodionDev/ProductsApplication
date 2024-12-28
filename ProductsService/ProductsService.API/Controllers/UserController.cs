using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.Common.Dto.Requests;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByUserId;

namespace ProductsService.API.Controllers
{
    [Controller]
    [Route("api/users")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetAllByUserId(Guid id, [FromQuery] FiltersRequestDto dto, CancellationToken cancellationToken)
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
    }
}
