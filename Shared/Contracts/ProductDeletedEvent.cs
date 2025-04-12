namespace Shared.Contracts
{
    public record ProductDeletedEvent(Guid ProductId) : IIntegrationEvent;
}
