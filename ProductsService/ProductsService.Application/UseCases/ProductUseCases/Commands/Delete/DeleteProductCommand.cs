using ProductsService.Application.Common.Abstractions;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Delete
{
    public record DeleteProductCommand(
        Guid ProductId,
        Guid UserId) : ICacheInvalidationCommand
    {
        public string Key => $"product:{ProductId}";
    }
}
