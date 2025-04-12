namespace UserService.Contracts.Outbox.Events
{
    public class UserActivated : IDomainEvent
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
