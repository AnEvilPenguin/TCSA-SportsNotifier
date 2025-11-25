using System.Net.Mail;
using Microsoft.Extensions.Options;
using SportsNotifierService.Model;

namespace SportsNotifierService.Services;

public class SmtpService (IOptions<SmtpOptions> settings) : ISmtpService
{
    private readonly SmtpOptions _settings = settings.Value;
    
    public void FakeEmail()
    {
        using var mail = new MailMessage();
        
        mail.From = new MailAddress(_settings.From);
        mail.To.Add(new MailAddress(_settings.To));
        
        mail.Subject = "Sports Notifier Service";
        
        mail.Body = "Hello World!";
        
        using var smtp = new SmtpClient(_settings.Server, _settings.Port);
        
        if (!string.IsNullOrEmpty(_settings.Username) && !string.IsNullOrEmpty(_settings.Password))
            smtp.Credentials = new System.Net.NetworkCredential(_settings.Username, _settings.Password);
        
        smtp.EnableSsl = _settings.EnableSsl;
        
        smtp.Send(mail);
    }
}