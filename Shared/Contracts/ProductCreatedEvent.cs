namespace Shared.Contracts
{
    public record ProductCreatedEvent(Guid ProductId) : IIntegrationEvent;
}
