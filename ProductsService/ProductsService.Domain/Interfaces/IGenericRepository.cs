using ProductsService.Domain.Abstrctions;

namespace ProductsService.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> 
        where TEntity : BaseModel
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void UpdateMany(List<TEntity> entities);
    }
}
