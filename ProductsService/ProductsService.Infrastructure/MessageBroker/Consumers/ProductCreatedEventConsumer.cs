using MassTransit;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductsService.Domain.Models;
using ProductsService.Persistence.Interfaces;
using ProductsService.Application.Common.Contracts;
using System.Threading;

namespace ProductsService.Infrastructure.MessageBroker.Consumers
{
    public sealed class ProductCreatedEventConsumer(
        IMongoCommandContext commandContext,
        IMongoQueryContext queryContext) : IConsumer<ProductCreated>
    {
        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var product = await commandContext
                .GetCollection<Product>(nameof(Product))
                .AsQueryable()
                .FirstOrDefaultAsync(p => p.Id == context.Message.ProductId);
            
            if (product is null)
            {
                return;
            }

            await queryContext
                .GetCollection<Product>(nameof(Product))
                .InsertOneAsync(product);
        }
    }
}
