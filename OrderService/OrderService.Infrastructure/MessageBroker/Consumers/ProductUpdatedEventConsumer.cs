using MassTransit;
using OrderService.Domain.Interfaces;
using Shared.Contracts;

namespace OrderService.Infrastructure.MessageBroker.Consumers
{
    internal sealed class ProductUpdatedEventConsumer(
        IUnitOfWork unitOfWork) : IConsumer<ProductUpdatedEvent>
    {
        public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
        {
            var items = await unitOfWork.BasketItemRepository
                .GetByProductIdAndQuantityAsync(
                    context.Message.ProductId,
                    context.Message.Quantity);

            foreach (var item in items)
            {
                item.Quantity = context.Message.Quantity;
                unitOfWork.BasketItemRepository.Update(item);
            }

            await unitOfWork.SaveChangesAsync();
        }
    }
}
