namespace ProductsService.Persistence.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(
            IQueryable<TEntity> query,
            Specification<TEntity> specification)
            where TEntity : class
        {
            IQueryable<TEntity> queryable = query;

            foreach (var criteria in specification.Criteria)
            {
                queryable = queryable.Where(criteria);
            }

            if(specification.OrderByExpression is not null)
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
