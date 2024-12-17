using OrderService.Domain.Abstractions;

namespace OrderService.Domain.Interfaces
{
    public interface IGenericRepository<T>
        where T : BaseModel
    {
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task CreateAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Delete(T entity);
    }
}
