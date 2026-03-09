using KRT.Services.Accounts.Core.Events;

namespace KRT.Services.Accounts.Infrastructure.MessageBus;

public interface IEventProcessor
{
    Task ProcessAsync(IEnumerable<IDomainEvent> events);
}
