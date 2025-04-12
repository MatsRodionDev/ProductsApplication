using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductsService.Domain.Abstrctions;
using ProductsService.Persistence.Interfaces;

namespace ProductsService.Persistence.Repositories
{
    public class GenericRepository<TEntity>
        where TEntity : IBaseModel
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<TEntity> _dbSet;

        public GenericRepository(IMongoContext context)
        {
            _context = context;
            _dbSet = _context.GetCollection<TEntity>(nameof(TEntity));
        }

        public virtual async Task AddAsync(TEntity product, CancellationToken cancellationToken = default)
        {
            await _dbSet.InsertOneAsync(product, cancellationToken: cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity product, CancellationToken cancellationToken = default)
        {
            await _dbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq(e => e.Id, product.Id), product, cancellationToken: cancellationToken);
        }

        public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _dbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq(e => e.Id, id), cancellationToken: cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsQueryable()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public virtual async Task UpdateManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var bulkOps = entities.Select(entity =>
                 new ReplaceOneModel<TEntity>(
                     Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id),
                     entity));

            await _dbSet.BulkWriteAsync(bulkOps, cancellationToken: cancellationToken);
        }
    }
}
