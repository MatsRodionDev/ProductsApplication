using MediatR;

namespace ProductsService.Application.Common.Events.Product
{
    public record ProductUpdatedEvent(Guid ProductId) : INotification;
}
