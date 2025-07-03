using System.Data;

namespace ProductsService.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IProductCommandRepository Products { get; }
        ICategoryCommandRepository Categories { get; }
        IOutboxRepository Outboxes { get; }

        Task<ITransaction> BeginTransactionAsync(IsolationLevel isolation = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
