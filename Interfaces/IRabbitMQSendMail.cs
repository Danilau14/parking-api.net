namespace ParkingApi.Interfaces;

public interface IRabbitMQSendMail
{
    public Task PublishAuditMessageAsync(
        string emailFromUser,
        string subject,
        string messageForUser
    );
}
