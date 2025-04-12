using MassTransit;
using Shared.Contracts;
using ProductsService.Domain.Interfaces;
using ProductsService.Infrastructure.Interfaces;

namespace ProductsService.Infrastructure.MessageBroker.Consumers
{
    public sealed class ProductUpdatedEventConsumer(
        IProductCommandRepository commandRepository,
        IUpdatableProductQueryRepository queryRepository) : IConsumer<ProductUpdatedEvent>
    {
        public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
        {
            var product = await commandRepository.GetByIdAsync(context.Message.ProductId);

            if (product is null)
            {
                return;
            }

            await queryRepository.UpdateAsync(product);
        }
    }
}
