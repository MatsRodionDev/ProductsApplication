using MediatR;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Application.Common.Contracts;

namespace ProductsService.Application.Common.Events.Product
{
    public class ProductUpdatedEventHandler(IEventBus eventBus) : INotificationHandler<ProductUpdatedEvent>
    {
        public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await eventBus.PublishAsync(new ProductUpdated(notification.ProductId), cancellationToken);
        }
    }
}
