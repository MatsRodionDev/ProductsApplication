using MassTransit;
using ProductsService.Application.Common.Contracts;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Infrastructure.MessageBroker.Consumers
{
    public sealed class ProductCreatedEventConsumer(
        IProductCommandRepository commandRepository,
        IProductQueryRepository queryRepository) : IConsumer<ProductCreated>
    {
        public async Task Consume(ConsumeContext<ProductCreated> context)
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
