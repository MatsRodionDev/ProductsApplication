using MediatR;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Application.Common.Contracts;

namespace ProductsService.Application.Common.Events.Product
{
    public class ProductCreatedEventHandler(IEventBus eventBus) : INotificationHandler<ProductCreatedEvent>
    {
        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            await eventBus.PublishAsync(new ProductCreated(notification.ProductId), cancellationToken);
        }
    }
}
