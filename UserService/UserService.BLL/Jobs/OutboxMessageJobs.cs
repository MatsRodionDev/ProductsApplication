using Microsoft.EntityFrameworkCore;
using UserService.BLL.Interfaces.MessageBroker;
using UserService.DAL;
using UserService.DAL.Outbox;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using Shared.Contracts;
using System.Text.Json;

namespace UserService.BLL.Jobs
{
    public sealed class OutboxMessageJobs(
        IServiceProvider serviceProvider,
        IEventBus eventBus)
    {
        private const int BatchSize = 1000;

        private static readonly ConcurrentDictionary<string, Type> TypeCache = new();

        public async Task ProcessOutboxMessagesJob()
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var sql = """
                        SELECT * 
                        FROM "Outboxes" 
                        WHERE "ProcessedOnUtc" IS NULL 
                        ORDER BY "OccuredOnUtc" 
                        FOR UPDATE SKIP LOCKED
                        LIMIT {0}
                        """;

                var messages = await context
                    .Outboxes
                    .FromSqlRaw(sql, BatchSize)
                    .AsTracking()
                    .ToListAsync();

                foreach (var message in messages)
                {
                    await PublishMessageAsync(message);
                }

                context.Outboxes.UpdateRange(messages);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            } 
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task PublishMessageAsync(OutboxMessageEntity message)
        {
            var messageType = GetOrAddMessageType(message.Type);

            if (messageType is null)
            {
                Console.WriteLine($"Unknown event type: {message.Type}");
                return;
            }

            var integrationEvent = JsonSerializer.Deserialize(message.Content, messageType);

            if (integrationEvent is null)
            {
                return;
            }

            try
            {
                await eventBus.PublishAsync(integrationEvent, messageType);

                message.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static Type? GetOrAddMessageType(string typeName)
        {
            var type = Shared.AssemblyReference.Assembly.GetType(typeName);

            if(type is null)
            {
                return null;
            }

            return TypeCache.GetOrAdd(typeName, type);
        }
    }
}
