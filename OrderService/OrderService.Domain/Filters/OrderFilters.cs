using OrderService.Domain.Enums;

namespace OrderService.Domain.Filters
{
    public record OrderFilters(
        string? ProductName,
        OrderStatus? OrderStatus,
        int Page,
        int PageSize);
}
