using ProductsService.Domain.Enums;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Models;
using System.Linq.Expressions;

namespace ProductsService.Persistence.Specifications
{
    public class GetProductsByFiltersSpecification : Specification<Product>
    {
        public GetProductsByFiltersSpecification(
            GetProductsFilters filters) 
        {
            AddCriteria(p => p.Name.Contains(filters.ProductName, StringComparison.CurrentCultureIgnoreCase));

            if (filters.Category is not null)
            {
                AddCriteria(p => p.Categories.Any(c => c.Name == filters.Category));
            }

            switch (filters.OrderBy)
            {
                case OrderBy.Id:
                    AddOrderByOrBy(p => p.Id, filters.Asc);
                    break;

                case OrderBy.Name:
                    AddOrderByOrBy(p => p.Name, filters.Asc);
                    break;

                case OrderBy.Price:
                    AddOrderByOrBy(p => p.Price, filters.Asc);
                    break;
            }
        }

        private void AddOrderByOrBy(Expression<Func<Product, object>> predicate, bool asc)
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
