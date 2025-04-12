using Shared.Contracts;

namespace UserService.BLL.Interfaces.MessageBroker
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
            where T : class;

        Task PublishAsync(object message, Type messageType, CancellationToken cancellationToken = default);
    }
}
