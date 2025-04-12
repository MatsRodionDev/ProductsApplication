using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Requests;
using ProductsService.Application.Common.Dto.Responses;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Take
{
    public record TakeProductsCommand(
        List<TakeProductRequestDto> Products) : ICommand<List<TakedProductResponseDto>>;
}
