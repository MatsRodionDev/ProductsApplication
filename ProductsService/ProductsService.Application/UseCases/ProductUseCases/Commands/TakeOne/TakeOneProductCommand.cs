using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.TakeOne
{
    public record TakeOneProductCommand(
        Guid Id,
        int Quantity) : ICommand<TakedProductResponseDto>;
}
