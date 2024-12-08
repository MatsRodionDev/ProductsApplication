using MediatR;

namespace ProductsService.Application.Common.Events.Product
{
    public record ProductCreatedEvent(Guid ProductId) : INotification;
}
