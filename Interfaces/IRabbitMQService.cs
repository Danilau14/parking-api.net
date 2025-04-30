namespace ParkingApi.Interfaces;

public interface IRabbitMQService
{
    public Task PublishMessage(MessageDto message, string? queueName = null);
    public Task<string> ConsumeMessages(string? queueName = null);
}
