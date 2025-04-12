using Microsoft.AspNetCore.Http;
using ProductsService.Application.Common.Abstractions;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.CreateImage
{
    public record CreateImageToProductCommand(
        Guid ProductId,
        Guid UserId,
        IFormFile File) : ICacheInvalidationCommand
    {
        public string Key => $"product:{ProductId}";
    }
}
