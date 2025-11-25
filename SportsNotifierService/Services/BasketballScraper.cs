using HtmlAgilityPack;
using SportsNotifierService.Model;

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
            _doc = await web.LoadFromWebAsync(Address);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading webpage");
            return false;
        }

        return true;
    }

    public List<MatchSummary> GetBasketballScores()
    {
        var output = new List<MatchSummary>();
        
        if (_doc == null)
            return output;

        var summaries = _doc.DocumentNode?.SelectSingleNode("//div[@class='game_summaries']");
        
        if (summaries == null)
            return output;
        
        var scores = summaries?.SelectNodes("//table[@class='teams']");
        
        if (scores == null)
            return output;

        foreach (var score in scores)
        {
            var home = ExtractTeamPerformance(score.SelectSingleNode("./tbody/tr[1]"));
            var away = ExtractTeamPerformance(score.SelectSingleNode("./tbody/tr[2]"));

            if (home == null || away == null)
            {
                logger.LogWarning("Failed to find home or away team for basketball score");
                logger.LogDebug("Score: {ScoreHTML}", score?.InnerHtml);
                
                continue;
            }
            
            var gameLink = score.SelectSingleNode("//td[contains(@class, 'gamelink')]/a");

            var isFinal = gameLink?.InnerText == "Final";

            output.Add(new MatchSummary
            {
                Home = home,
                Away = away,
                IsFinal = isFinal,
                Link = gameLink?.Attributes["href"]?.Value,
            });
        }
        
        return  output;
    }

    private TeamPerformance? ExtractTeamPerformance (HtmlNode? htmlNode)
    {
        if (htmlNode == null)
            return  null;
        
        var tds = htmlNode?.SelectNodes("./td");
        
        if (tds?.Count < 2)
            return null;
        
        var team = tds[0];
        var teamName = team.FirstChild.InnerText;
        var teamLink = team.FirstChild.Attributes["href"].Value;
        
        var score = tds[1].InnerText;
        
        return new TeamPerformance
        {
            TeamName = teamName,
            Score = int.Parse(score),
            Link = teamLink,
        };
    }
}