namespace ParkingApi.Interfaces;

public interface IEmailSettings
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}
