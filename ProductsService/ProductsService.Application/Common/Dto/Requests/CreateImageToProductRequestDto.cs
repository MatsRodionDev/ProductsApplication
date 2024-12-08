using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.Common.Dto.Requests
{
    public record CreateImageToProductRequestDto(
        IFormFile File,
        Guid UserId);
}
