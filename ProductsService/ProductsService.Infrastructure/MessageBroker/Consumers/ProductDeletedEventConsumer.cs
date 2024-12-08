using MassTransit;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductsService.Domain.Models;
using ProductsService.Persistence.Interfaces;
using ProductsService.Application.Common.Contracts;

namespace ProductsService.Infrastructure.MessageBroker.Consumers
{
    public sealed class ProductDeletedEventConsumer(
        IMongoCommandContext commandContext,
        IMongoQueryContext queryContext) : IConsumer<ProductDeleted>
    {
        public async Task Consume(ConsumeContext<ProductDeleted> context)
        {
            var product = await commandContext
                .GetCollection<Product>(nameof(Product))
                .AsQueryable()
                .FirstOrDefaultAsync(p => p.Id == context.Message.ProductId);

            if(product is not  null)
            {
                return;
            }

            await queryContext
                .GetCollection<Product>(nameof(Product))
                .DeleteOneAsync(Builders<Product>.Filter.Eq(e => e.Id, context.Message.ProductId));
        }
    }
}
