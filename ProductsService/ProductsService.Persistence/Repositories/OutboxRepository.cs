using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ProductsService.Persistence.Repositories
{
    internal sealed class OutboxRepository(ApplicationDBContext context) : IOutboxRepository
    {
        public async Task<List<Outbox>> GetForUpdateAsync(int batchSize, CancellationToken cancellationToken = default)
        {
            return await context.Outboxes
                .FromSqlRaw(
                    """
                    SELECT * 
                    FROM "Outboxes"
                    ORDER BY "CreatedAt"
                    TAKE @batchSize
                    FOR UPDATE
                    SKIP UPDATED
                    """, batchSize)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync<T>(T eventModel, CancellationToken cancellationToken = default)
        {
            await context.Outboxes
                .AddAsync(new Outbox
                {
                    Content = JsonSerializer.Serialize(eventModel),
                    Type = typeof(T).Name,
                    CreatedAt = DateTime.UtcNow
                }, cancellationToken);
        }


        public void UpdateMany(List<Outbox> outboxes)
        {
            context.Outboxes
                .UpdateRange(outboxes);
        }
    }
}
