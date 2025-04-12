namespace ProductsService.Application.Common.Dto.Requests
{
    public record TakeProductRequestDto(
        Guid Id,
        int Quantity);
}
