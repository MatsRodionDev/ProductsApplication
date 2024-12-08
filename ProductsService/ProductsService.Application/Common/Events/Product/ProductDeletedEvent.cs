using MediatR;

namespace ProductsService.Application.Common.Events.Product
{
    public record ProductDeletedEvent(Guid ProductId) : INotification;
}
