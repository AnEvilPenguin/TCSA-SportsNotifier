using SportsNotifierService.Services;
using TickerQ.Utilities.Base;

namespace SportsNotifierService.Jobs;

public class BasketballJobs(ILogger<BasketballJobs> logger, IBasketballScraper scraper)
{
    [TickerFunction($"{nameof(BasketballJobs)}.{nameof(ScrapeBasketballResults)}", "0 0 16 * * *")]
    public async Task ScrapeBasketballResults()
    {
        logger.LogDebug("Starting ScrapeBasketballResults");

        if (await scraper.Load())
            logger.LogDebug("ScrapeBasketballResults loaded");
        else
            logger.LogDebug("ScrapeBasketballResults not loaded");
    }
}