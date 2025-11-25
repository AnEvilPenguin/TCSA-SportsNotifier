using SportsNotifierService.Model;

namespace SportsNotifierService.Services;

public interface IBasketballScraper
{
    public Task<bool> Load();
    public List<MatchSummary> GetBasketballScores();
}