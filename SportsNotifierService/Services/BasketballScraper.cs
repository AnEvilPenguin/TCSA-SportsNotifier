using HtmlAgilityPack;

namespace SportsNotifierService.Services;

public class BasketballScraper(ILogger<BasketballScraper> logger) : IBasketballScraper
{
    const string Address = "https://www.basketball-reference.com/boxscores/";
    private HtmlDocument? _doc;
    
    public async Task<bool> Load()
    {
        var web = new HtmlWeb();

        try
        {
            var _doc = await web.LoadFromWebAsync(Address);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading webpage");
            return false;
        }

        return true;
    }
}