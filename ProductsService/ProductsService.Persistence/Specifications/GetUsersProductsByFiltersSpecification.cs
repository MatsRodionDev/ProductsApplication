using ProductsService.Domain.Enums;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Models;

namespace ProductsService.Persistence.Specifications
{
    public class GetUsersProductsByFiltersSpecification : Specification<Product>
    {
        public GetUsersProductsByFiltersSpecification(
            GetUsersProductsFilters filters) 
        {
            AddCriteria(p => p.UserId == filters.UserId);
            AddCriteria(p => p.Name.Contains(filters.ProductName, StringComparison.CurrentCultureIgnoreCase));

            if (filters.Category is not null)
            {
                AddCriteria(p => p.Categories.Any(c => c.Name == filters.Category));
            }

            if (filters.Asc)
            {
                switch (filters.OrderBy)
                {
                    case OrderBy.Id:
                        AddOrderBy(p => p.Id);
                        break;

                    case OrderBy.Name:
                        AddOrderBy(p => p.Name);
                        break;

                    case OrderBy.Price:
                        AddOrderBy(p => p.Price);
                        break;
                }
            }
            else
            {
                switch (filters.OrderBy)
                {
                    case OrderBy.Id:
                        AddOrderByDescending(p => p.Id);
                        break;

                    case OrderBy.Name:
                        AddOrderByDescending(p => p.Name);
                        break;

                    case OrderBy.Price:
                        AddOrderByDescending(p => p.Price);
                        break;
                }
            }
        }
    }
}
