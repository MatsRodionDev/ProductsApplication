using MassTransit;
using ProductsService.Application.Common.Contracts;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Infrastructure.MessageBroker.Consumers
{
    public sealed class ProductUpdatedEventConsumer(
        IProductCommandRepository commandRepository,
        IProductQueryRepository queryRepository) : IConsumer<ProductUpdated>
    {
        public async Task Consume(ConsumeContext<ProductUpdated> context)
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
