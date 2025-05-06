namespace ParkingApi.Application.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var mail = new MailMessage
        {
            From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };

        mail.To.Add(toEmail);

        using var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
        {
            Credentials = new NetworkCredential(_emailSettings.SenderUser, _emailSettings.Password),
            EnableSsl = true
        };

        await smtp.SendMailAsync(mail);
    }
}
