using MassTransit;
using ProductsService.Application.Common.Interfaces;

namespace ProductsService.Infrastructure.MessageBroker
{
    public class EventBus(IPublishEndpoint publishEndpoint) : IEventBus
    {
        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) 
            where T : class =>
            await publishEndpoint.Publish(message, cancellationToken);
    }
}
