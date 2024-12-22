using MassTransit;
using ProductsService.Application.Common.Contracts;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Infrastructure.MessageBroker.Consumers
{
    public sealed class ProductDeletedEventConsumer(
        IProductCommandRepository commandRepository,
        IProductQueryRepository queryRepository) : IConsumer<ProductDeleted>
    {
        public async Task Consume(ConsumeContext<ProductDeleted> context)
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
