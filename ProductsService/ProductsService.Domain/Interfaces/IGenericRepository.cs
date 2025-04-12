using ProductsService.Domain.Abstrctions;

namespace ProductsService.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> 
        where TEntity : IBaseModel
    {
        Task AddAsync(TEntity product, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity product, CancellationToken cancellationToken = default);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
    }
}
