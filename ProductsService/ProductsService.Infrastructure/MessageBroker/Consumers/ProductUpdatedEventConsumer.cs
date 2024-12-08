using MassTransit;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductsService.Domain.Models;
using ProductsService.Persistence.Interfaces;
using ProductsService.Application.Common.Contracts;

namespace ProductsService.Infrastructure.MessageBroker.Consumers
{
    public sealed class ProductUpdatedEventConsumer(
        IMongoCommandContext commandContext,
        IMongoQueryContext queryContext) : IConsumer<ProductUpdated>
    {
        public async Task Consume(ConsumeContext<ProductUpdated> context)
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
                .ReplaceOneAsync(Builders<Product>.Filter.Eq(e => e.Id, product.Id), product);
        }
    }
}
