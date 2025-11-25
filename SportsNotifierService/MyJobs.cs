using TickerQ.Utilities.Base;

namespace SportsNotifierService;

public class MyJobs
{
    private readonly ILogger<MyJobs> _logger;
    private readonly BasketballScraper _basketballScraper = new ();

    public MyJobs(ILogger<MyJobs> logger)
    {
        _logger = logger;
    }
    
    [TickerFunction("ScrapeBasketballResults", "0 0 16 * * *")]
    public void ScrapeBasketballResults()
    {
        _logger.LogInformation("Starting ScrapeBasketBallResults");
        
        _basketballScraper.Load();
        _logger.LogInformation("ScrapeBasketballResults finished");
    }
}