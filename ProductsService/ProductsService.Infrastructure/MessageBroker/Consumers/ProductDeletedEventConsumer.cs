using MassTransit;
using ProductsService.Application.Common.Contracts;
using ProductsService.Domain.Interfaces;
using ProductsService.Infrastructure.Interfaces;

namespace ProductsService.Infrastructure.MessageBroker.Consumers
{
    public sealed class ProductDeletedEventConsumer(
        IProductCommandRepository commandRepository,
        IUpdatableProductQueryRepository queryRepository) : IConsumer<ProductDeletedEvent>
    {
        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            var product = await commandRepository.GetByIdAsync(context.Message.ProductId);

            if (product is not null)
            {
                return;
            }

            await queryRepository.RemoveAsync(context.Message.ProductId);
        }
    }
}
