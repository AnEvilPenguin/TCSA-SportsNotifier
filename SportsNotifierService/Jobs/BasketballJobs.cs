using SportsNotifierService.Services;
using TickerQ.Utilities.Base;

namespace SportsNotifierService.Jobs;

public class BasketballJobs(ILogger<BasketballJobs> logger, IBasketballScraper scraper)
{
    [TickerFunction($"{nameof(BasketballJobs)}.{nameof(ScrapeBasketballResults)}", "0 0 16 * * *")]
    public async Task ScrapeBasketballResults()
    {
        logger.LogDebug("Starting ScrapeBasketballResults");

        if (!await scraper.Load())
        {
            logger.LogError("ScrapeBasketballResults - Document not loaded");
            return;
        }
            
        var scores = scraper.GetBasketballScores();

        foreach (var score in scores)
        {
            Console.WriteLine($"{score.Home.TeamName}: {score.Home.Score}");
            Console.WriteLine($"{score.Away.TeamName}: {score.Away.Score}");
            
            if (score.IsFinal)
                Console.WriteLine($"Final");
            
            Console.WriteLine();
        }
    }
}