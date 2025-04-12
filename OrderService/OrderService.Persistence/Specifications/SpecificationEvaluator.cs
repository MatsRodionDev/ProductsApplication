using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Abstractions;

namespace OrderService.Persistence.Specifications
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(
            IQueryable<TEntity> query,
            Specification<TEntity> specification)
            where TEntity : BaseModel
        {
            IQueryable<TEntity> queryable = query;

            queryable = specification.IncludeExpression.Aggregate(
                queryable, 
                (current, include) => current.Include(include));

            foreach (var criteria in specification.Criteria)
            {
                queryable = queryable.Where(criteria);
            }

            if (specification.OrderByExpression is not null)
            {
                return queryable.OrderBy(specification.OrderByExpression);
            }

            if (specification.OrderByDescendingExpression is not null)
            {
                return queryable.OrderByDescending(specification.OrderByDescendingExpression);
            }

            return queryable;
        }
    }
}
