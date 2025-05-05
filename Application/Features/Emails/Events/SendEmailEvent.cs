namespace ParkingApi.Application.Features.Emails.Events;

public class SendEmailEvent : INotification
{
    public string Email { get; }
    public string Subject { get; }
    public string Message { get; }

    public SendEmailEvent(string email, string subject, string message)
    {
        Email = email;
        Subject = subject;
        Message = message;
    }
}
