using System.Linq.Expressions;

namespace ProductsService.Persistence.Specifications
{
    public abstract class Specification<TEntity>
        where TEntity : class
    {
        public List<Expression<Func<TEntity, bool>>> Criteria { get; } = [];

        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

        public Expression<Func<TEntity, object>>? OrderByExpression { get; private set;  }

        public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

        protected void AddCriteria(Expression<Func<TEntity, bool>> criteria) =>
            Criteria.Add(criteria);

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) =>
            IncludeExpressions.Add(includeExpression);

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderBy) =>
            OrderByExpression = orderBy;

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending) =>
            OrderByDescendingExpression = orderByDescending;
    }
}
