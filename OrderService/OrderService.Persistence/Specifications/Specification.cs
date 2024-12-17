using OrderService.Domain.Abstractions;
using System.Linq.Expressions;

namespace OrderService.Persistence.Specifications
{
    internal abstract class Specification<TEntity>
        where TEntity : BaseModel
    {
        public List<Expression<Func<TEntity, bool>>> Criteria { get; } = [];

        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; private set; } = [];

        public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

        protected void AddCriteria(Expression<Func<TEntity, bool>> criteria) =>
            Criteria.Add(criteria);

        protected void AddInclude(Expression<Func<TEntity, object>> include)
            => IncludeExpression.Add(include);

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderBy) =>
            OrderByExpression = orderBy;

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending) =>
            OrderByDescendingExpression = orderByDescending;
    }
}
