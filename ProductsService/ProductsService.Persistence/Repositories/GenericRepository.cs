using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstrctions;

namespace ProductsService.Persistence.Repositories
{
    public class GenericRepository<TEntity>(
        ApplicationDBContext context)
        where TEntity : BaseModel
    {
        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await context.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public virtual void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context
                .Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public virtual void UpdateMany(List<TEntity> entities)
        {
            context.Set<TEntity>().UpdateRange(entities);
        }
    }
}
