namespace ParkingApi.Application.Features.Emails.Handlers;

public class SendEmailEventHandler : INotificationHandler<SendEmailEvent>
{
    private readonly IRabbitMQSendMail _rabbitMQSendMail;

    public SendEmailEventHandler(IRabbitMQSendMail sendMail) 
    {  
        _rabbitMQSendMail = sendMail; 
    }

    public async Task Handle(SendEmailEvent notification, CancellationToken cancellationToken)
    {
        await _rabbitMQSendMail.PublishAuditMessageAsync(
            notification.Email,
            notification.Subject,
            notification.Message
        );
    }
}
