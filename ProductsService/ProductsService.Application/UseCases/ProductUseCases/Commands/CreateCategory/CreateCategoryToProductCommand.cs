using ProductsService.Application.Common.Abstractions;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.CreateCategory
{
    public record CreateCategoryToProductCommand(
        Guid ProductId,
        Guid UserId,
        Guid CategoryId) : ICacheInvalidationCommand
    {
        public string Key => $"product:{ProductId}";
    }
}
