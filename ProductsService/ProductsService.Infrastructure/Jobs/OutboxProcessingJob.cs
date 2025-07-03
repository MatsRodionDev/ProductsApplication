using ProductsService.Application.Common.Interfaces;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using Shared.Contracts;
using System.Collections.Concurrent;
using System.Text.Json;

namespace ProductsService.Infrastructure.Jobs
{
    internal sealed class OutboxProcessingJob(
        IUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        private const int BATCH_SIZE = 100;

        private static readonly ConcurrentDictionary<string, Type> _eventTypeDictionary = [];

        public async Task ProcessOutboxes(CancellationToken cancellationToken)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

            var outboxes = await unitOfWork.Outboxes.GetForUpdateAsync(BATCH_SIZE, cancellationToken);

            await Parallel.ForEachAsync(
                outboxes,
                new ParallelOptions { MaxDegreeOfParallelism = 4, CancellationToken = cancellationToken },
                    async (outbox, ct) =>
                    {
                        await ProcessOutbox(outbox, ct);
                    });

            unitOfWork.Outboxes.UpdateMany(outboxes);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

        private async Task ProcessOutbox(Outbox outbox, CancellationToken cancellationToken)
        {
            try
            {
                var type = GetOrAddType(outbox.Type);

                var integrationEvent = JsonSerializer.Deserialize(outbox.Content, type)!;

                await eventBus.PublishAsync(integrationEvent, cancellationToken);

                outbox.ProcessedAt = DateTime.UtcNow;
            }
            catch(Exception ex) 
            {
                outbox.Error = ex.Message;
            }
        }

        private static Type GetOrAddType(string stringType)
        {
            return _eventTypeDictionary.GetOrAdd(stringType, type
                => typeof(IIntegrationEvent).Assembly.GetType(type)!);
        }
    }
}
