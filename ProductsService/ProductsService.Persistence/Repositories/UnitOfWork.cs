using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProductsService.Domain.Interfaces;
using System.Data;

namespace ProductsService.Persistence.Repositories
{
    internal sealed class UnitOfWork(
        ApplicationDBContext context,
        IProductCommandRepository products,
        ICategoryCommandRepository categories,
        IOutboxRepository outboxes) : IUnitOfWork
    {
        public IProductCommandRepository Products => products;
        public ICategoryCommandRepository Categories => categories;
        public IOutboxRepository Outboxes => outboxes;

        public async Task<ITransaction> BeginTransactionAsync(IsolationLevel isolation, CancellationToken cancellationToken)
        { 
            var transaction = await context.Database.BeginTransactionAsync(isolation, cancellationToken);

            return new Transaction(transaction);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return context.DisposeAsync();
        }
    }

    internal sealed class Transaction(IDbContextTransaction transaction) : ITransaction
    {
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            await transaction.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            transaction.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return transaction.DisposeAsync();
        }
    }
}
