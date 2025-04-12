using Microsoft.AspNetCore.Http;
using ProductsService.Domain.Enums;

namespace ProductsService.Application.Common.Dto.Requests
{
    public record CreateProductRequestDto(
        string Name,
        string Description,
        int Quantity,
        int Price,
        List<IFormFile>? ImageFiles,
        List<Categories>? Categories);
}
