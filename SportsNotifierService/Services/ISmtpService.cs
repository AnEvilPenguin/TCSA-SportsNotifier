using MimeKit;

namespace SportsNotifierService.Services;

public interface ISmtpService
{
    public Task SendEmailAsync(MimeMessage message);
}