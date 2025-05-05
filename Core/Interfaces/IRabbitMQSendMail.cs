namespace ParkingApi.Core.Interfaces;

public interface IRabbitMQSendMail
{
    public Task PublishAuditMessageAsync(
        string emailFromUser,
        string subject,
        string messageForUser
    );
}
