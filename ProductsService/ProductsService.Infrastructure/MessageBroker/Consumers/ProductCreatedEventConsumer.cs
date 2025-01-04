using MassTransit;
using ProductsService.Application.Common.Contracts;
using ProductsService.Domain.Interfaces;
using ProductsService.Infrastructure.Interfaces;

namespace ProductsService.Infrastructure.MessageBroker.Consumers
{
    public sealed class ProductCreatedEventConsumer(
        IProductCommandRepository commandRepository,
        IUpdatableProductQueryRepository queryRepository) : IConsumer<ProductCreatedEvent>
    {
        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            var product = await commandRepository.GetByIdAsync(context.Message.ProductId);
            
            if (product is null)
            {
                return;
            }

            await queryRepository.AddAsync(product);
        }
    }
}
