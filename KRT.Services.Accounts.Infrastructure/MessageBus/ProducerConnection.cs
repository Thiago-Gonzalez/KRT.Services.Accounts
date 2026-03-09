using RabbitMQ.Client;

namespace KRT.Services.Accounts.Infrastructure.MessageBus;

public class ProducerConnection
{
    public ProducerConnection(IConnection connection)
    {
        Connection = connection;
    }

    public IConnection Connection { get; private set; }
}
