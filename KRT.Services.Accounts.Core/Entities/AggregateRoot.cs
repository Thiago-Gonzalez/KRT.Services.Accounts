using KRT.Services.Accounts.Core.Events;

namespace KRT.Services.Accounts.Core.Entities;

public class AggregateRoot : IEntityBase
{
    public int Id {  get; protected set; }

    private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
    public IEnumerable<IDomainEvent> Events => _events;

    protected void AddEvent(IDomainEvent @event) => _events.Add(@event);
}
