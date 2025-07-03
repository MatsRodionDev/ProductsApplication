namespace Shared.Contracts
{
    public record ProductDeletedEvent(Guid ProductId, string[] ImageNames) : IIntegrationEvent;
}
