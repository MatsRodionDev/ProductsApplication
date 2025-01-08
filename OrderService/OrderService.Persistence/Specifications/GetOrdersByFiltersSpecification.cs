using OrderService.Domain.Filters;
using OrderService.Domain.Models;

namespace OrderService.Persistence.Specifications
{
    internal class GetOrdersByFiltersSpecification : Specification<Order>
    {
        public GetOrdersByFiltersSpecification(
           OrderFilters filters)
        {
            AddInclude(o => o.OrderItem!);

            if(filters.ProductName is not null)
            {
                AddCriteria(o => o.OrderItem!.ProdcutName == filters.ProductName);
            }

            if(filters.OrderStatus is not null)
            {
                AddCriteria(o => o.Status == filters.OrderStatus.ToString());
            }
        }
    }
}
