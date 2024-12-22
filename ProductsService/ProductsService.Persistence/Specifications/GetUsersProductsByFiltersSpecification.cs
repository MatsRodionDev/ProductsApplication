using ProductsService.Domain.Enums;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Models;
using System.Linq.Expressions;

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

            switch (filters.OrderBy)
            {
                case OrderBy.Id:
                    AddOrderByOrByDescending(p => p.Id, filters.Asc);
                    break;

                case OrderBy.Name:
                    AddOrderByOrByDescending(p => p.Name, filters.Asc);
                    break;

                case OrderBy.Price:
                    AddOrderByOrByDescending(p => p.Price, filters.Asc);
                    break;
            }
        }

        private void AddOrderByOrByDescending(Expression<Func<Product, object>> predicate, bool asc)
        {
            if (asc)
            {
                AddOrderBy(predicate);
            }
            else
            {
                AddOrderByDescending(predicate);
            }
        }
    }
}
