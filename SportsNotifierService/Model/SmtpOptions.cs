namespace SportsNotifierService.Model;

public record SmtpOptions
{
    public required string Server { get; set; }
    public int Port { get; set; } = 25;
    public bool EnableSsl { get; set; } = true;
};