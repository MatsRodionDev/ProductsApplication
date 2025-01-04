using ProductsService.Application.Common.Abstractions;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Update
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string Description,
        int Quantity,
        int Price,
        Guid UserId) : ICacheInvalidationCommand
    {
        public string Key => $"product:{Id}";
    }
}
