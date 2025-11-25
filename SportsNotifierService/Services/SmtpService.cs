using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SportsNotifierService.Model;

namespace SportsNotifierService.Services;

public class SmtpService (IOptions<SmtpOptions> settings) : ISmtpService
{
    private readonly SmtpOptions _settings = settings.Value;

    public async Task SendEmailAsync(MimeMessage message)
    {
        message.To.Add(new MailboxAddress(_settings.To, _settings.To));
        message.From.Add(new MailboxAddress("Sports Notifier", _settings.From));
        
        using var smtpClient = new SmtpClient();
        
        await smtpClient.ConnectAsync(_settings.Server, _settings.Port);
        
        if (!string.IsNullOrEmpty(_settings.Username) && !string.IsNullOrEmpty(_settings.Password))
            await smtpClient.AuthenticateAsync(_settings.Username, _settings.Password);

        await smtpClient.SendAsync(message);
    }
}