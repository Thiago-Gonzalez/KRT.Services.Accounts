using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KRT.Services.Accounts.Infrastructure.MessageBus;

public class RabbitMQClient : IMessageBusClient
{
    private readonly IConnection _connection;
    public RabbitMQClient(ProducerConnection producerConnection)
    {
        _connection = producerConnection.Connection;
    }

    public async Task PublishAsync(object message, string routingKey, string exchange)
    {
        await using var channel = await _connection.CreateChannelAsync();

        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var payload = JsonSerializer.Serialize(message, options);
        var body = Encoding.UTF8.GetBytes(payload);

        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, true);

        var queue = $"{routingKey}-queue";
        await channel.QueueDeclareAsync(queue, true, false, false);
        await channel.QueueBindAsync(queue, exchange, routingKey);

        await channel.BasicPublishAsync(exchange, routingKey, true, body);
    }
}
