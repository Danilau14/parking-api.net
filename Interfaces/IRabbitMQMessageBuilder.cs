namespace ParkingApi.Interfaces;

public interface IRabbitMQMessageBuilder
{
    public Task PublishAuditMessageAsync(
            string entity,
            Actions action,
            bool state,
            int? userId = null,
            string? response = null,
            string? queueName = null
        );
}
