namespace ProductsService.Application.Common.Dto.Requests
{
    public record UpdateProductRequestDto(
        string Name,
        string Description,
        int Quantity,
        int Price);
}
