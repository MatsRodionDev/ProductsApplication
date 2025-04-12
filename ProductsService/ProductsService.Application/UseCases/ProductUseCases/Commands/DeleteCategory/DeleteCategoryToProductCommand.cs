using ProductsService.Application.Common.Abstractions;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteCategory
{
    public record DeleteCategoryToProductCommand(
        Guid ProductId,
        Guid CategoryId,
        Guid UserId) : ICacheInvalidationCommand
    {
        public string Key => $"product:{ProductId}";
    }
}
