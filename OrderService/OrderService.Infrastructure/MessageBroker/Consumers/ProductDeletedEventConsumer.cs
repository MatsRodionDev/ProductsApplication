using MassTransit;
using OrderService.Domain.Interfaces;
using Shared.Contracts;

namespace OrderService.Infrastructure.MessageBroker.Consumers
{
    internal sealed class ProductDeletedEventConsumer(
        IUnitOfWork unitOfWork) : IConsumer<ProductDeletedEvent>
    {
        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            var items = await unitOfWork.BasketItemRepository
                .GetByProductIdAsync(context.Message.ProductId);

            unitOfWork.BasketItemRepository.DeleteRange(items);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
