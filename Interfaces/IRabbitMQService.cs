namespace ParkingApi.Interfaces;

public interface IRabbitMQService
{
    public Task PublishMessage<T>(T message, string? queueName = null);
    public Task<string> ConsumeMessages(string? queueName = null);
}
