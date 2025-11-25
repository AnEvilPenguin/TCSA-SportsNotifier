namespace SportsNotifierService.Model;

public record MatchSummary
{
    public required TeamPerformance Home { get; set; }
    public required TeamPerformance Away { get; set; }
    public bool IsFinal { get; set; }
    public string? Link { get; set; }
};