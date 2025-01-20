namespace Shared.Contracts
{
    public record ProductUpdatedEvent(
        Guid ProductId, 
        int Quantity);
}
