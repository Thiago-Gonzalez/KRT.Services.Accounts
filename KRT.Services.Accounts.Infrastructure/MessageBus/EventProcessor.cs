using KRT.Services.Accounts.Core.Events;
using System.Text;

namespace KRT.Services.Accounts.Infrastructure.MessageBus;

public class EventProcessor : IEventProcessor
{
    private readonly IMessageBusClient _bus;
    public EventProcessor(IMessageBusClient bus)
    {
        _bus = bus;
    }

    public async Task ProcessAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            await _bus.PublishAsync(@event, MapConvention(@event), "accounts-service");
        }
    }

    private string MapConvention(IDomainEvent @event) =>  ToDashCase(@event.GetType().Name);

    public string ToDashCase(string text)
    {
        if (text is null) throw new ArgumentNullException(nameof(text));

        if (text.Length < 2)
            return text;

        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));

        for (int i = 1; i < text.Length; ++i)
        {
            char c = text[i];
            if (char.IsUpper(c))
            {
                sb.Append('-');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}
