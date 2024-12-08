using MediatR;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Domain.Enums;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Create
{
    public record CreateProductCommand(
        string Name,
        string Description,
        int Quantity,
        int Price,
        Guid UserId,
        List<IFormFile>? ImageFiles,
        List<Categories>? Categories) : ICommand;
}
