using Shared.Contracts;

namespace UserService.DAL.Abstractions
{
    public class BaseEntity
    {
        private readonly List<IIntegrationEvent> _integartionEvents = new();

        public Guid Id { get; set; }

        public IReadOnlyCollection<IIntegrationEvent> GetEvents() =>
            _integartionEvents.ToList();

        public void ClearEvents() =>
            _integartionEvents.Clear();

        public void AddEvent(IIntegrationEvent integartionEvent) =>
            _integartionEvents.Add(integartionEvent);
    }
}
