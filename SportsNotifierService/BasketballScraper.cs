using HtmlAgilityPack;

namespace SportsNotifierService;

// TODO move to service
public class BasketballScraper
{
    const string Address = "https://www.basketball-reference.com/boxscores/";

    public void Load()
    {
        var web = new HtmlWeb();
        var doc = web.Load(Address);
        
        var node = doc.DocumentNode.SelectSingleNode("//div[@class='game_summaries']");
    }
}