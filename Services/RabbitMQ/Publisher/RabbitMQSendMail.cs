namespace ParkingApi.Services.RabbitMQ.Publisher;

public class RabbitMQSendMail : IRabbitMQSendMail
{
    private readonly IRabbitMQService _rabbitmqService;

    public RabbitMQSendMail(IRabbitMQService rabbitmqService)
    {
        _rabbitmqService = rabbitmqService;
    }

    public async Task PublishAuditMessageAsync(
        string emailFromUser, 
        string subject,
        string messageForUser
    )
    {
        var email = new EmailForUserDto
        {
            Email = emailFromUser,
            Subject = subject,
            Message = messageForUser
        };

        await _rabbitmqService.PublishMessage(email, "email");
    }
}
