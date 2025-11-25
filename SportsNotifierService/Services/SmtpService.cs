using Microsoft.Extensions.Options;
using SportsNotifierService.Model;

namespace SportsNotifierService.Services;

public class SmtpService (IOptions<SmtpOptions> settings) : ISmtpService
{
    private readonly SmtpOptions _settings = settings.Value;
    
    public void FakeEmail()
    {
        Console.WriteLine($"{_settings.Server}:{_settings.Port}");
    }
}