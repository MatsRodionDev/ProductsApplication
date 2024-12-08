using ProductsService.Application.Common.Abstractions;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteImage
{
    public record DeleteImageToProductCommand(
        Guid ProductId,
        Guid ImageId,
        Guid UserId) : ICacheInvalidationCommand
    {
        public string Key => $"product:{ProductId}";
    }

}
