namespace Shared.Contracts
{
    public record UserActivatedEvent(
        Guid UserId) : IIntegrationEvent;
}
