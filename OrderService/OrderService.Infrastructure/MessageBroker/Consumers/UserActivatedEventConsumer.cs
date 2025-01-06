using MassTransit;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using Shared.Contracts;

namespace OrderService.Infrastructure.MessageBroker.Consumers
{
    internal sealed class UserActivatedEventConsumer(
        IUnitOfWork unitOfWork) : IConsumer<UserActivatedEvent>
    {
        public async Task Consume(ConsumeContext<UserActivatedEvent> context)
        {
            var user = await unitOfWork.BasketRepository
                .GetByUserIdAsync(context.Message.UserId);

            if (user is not null)
            {
                return;
            }

            await unitOfWork.BasketRepository
                .CreateAsync(new Basket(context.Message.UserId));
            await unitOfWork.SaveChangesAsync();
        }
    }
}
