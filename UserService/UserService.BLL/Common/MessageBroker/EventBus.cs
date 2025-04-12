using MassTransit;
using Shared.Contracts;
using UserService.BLL.Interfaces.MessageBroker;

namespace UserService.BLL.Common.MessageBroker
{
    public class EventBus(IPublishEndpoint publishEndpoint) : IEventBus
    {
        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
            where T : class =>
            await publishEndpoint.Publish(message, cancellationToken);

        public async Task PublishAsync(object message, Type messageType, CancellationToken cancellationToken = default) =>
            await publishEndpoint.Publish(message, messageType, cancellationToken);
    }
}
