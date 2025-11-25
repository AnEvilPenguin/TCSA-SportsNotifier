namespace SportsNotifierService.Model;

public record TeamPerformance
{
    public required string TeamName { get; set; }
    public int Score { get; set; }
    public string? Link { get; set; }
}