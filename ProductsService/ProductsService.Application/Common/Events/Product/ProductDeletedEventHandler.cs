using MediatR;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Application.Common.Contracts;

namespace ProductsService.Application.Common.Events.Product
{
    internal sealed class ProductDeletedEventHandler(IEventBus bus) : INotificationHandler<ProductDeletedEvent>
    {
        public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
        {
            await bus.PublishAsync(new ProductDeleted(notification.ProductId), cancellationToken);
        }
    }
}
