using ParkingApi.Core.Interfaces;
using ParkingApi.Core.Models;
using RabbitMQ.Client.Events;

namespace ParkingApi.Application.Services.RabbitMQ;

public class RabbitMQService : IRabbitMQService
{
    private readonly RabbitMQSettings _rabbitMQSettings;
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMQService(IOptions<RabbitMQSettings> rabbitMQSettings)
    {
        _rabbitMQSettings = rabbitMQSettings.Value;
    }

    private async Task EnsureConnectionAndChannelAsync()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitMQSettings.HostName,
                UserName = _rabbitMQSettings.UserName,
                Password = _rabbitMQSettings.Password,
                Port = _rabbitMQSettings.Port
            };

            _connection = await connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }
    }

    public async Task PublishMessage<T>(T message, string? queueName = null)
    {
        await EnsureConnectionAndChannelAsync();

        await _channel.QueueDeclareAsync(
            queueName ?? _rabbitMQSettings.QueueName, 
            durable: true, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null
        );

        var messageSerilized = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(messageSerilized);

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName ?? _rabbitMQSettings.QueueName,
            body: body
           );

        Console.WriteLine($" [x] Sent {message}");
    }

    public async Task<string> ConsumeMessages(string? queueName = null)
    {
        await EnsureConnectionAndChannelAsync();

        await _channel.QueueDeclareAsync(
            queueName ?? _rabbitMQSettings.QueueName, 
            durable: true, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);
        var message = string.Empty;

        consumer.ReceivedAsync += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
            return Task.CompletedTask; 
        };

        await _channel.BasicConsumeAsync(queueName ?? _rabbitMQSettings.QueueName, autoAck: true, consumer: consumer);

        return message;
    }
}
