namespace SportsNotifierService.Model;

public record SmtpOptions
{
    public required string Server { get; init; }
    public int Port { get; init; } = 25;
    public bool EnableSsl { get; init; } = true;
    public string? Username { get; init; }
    public string? Password { get; init; }
    public required string To { get; init; }
    public required string From { get; init; }
};