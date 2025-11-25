using System.Xml;
using MimeKit;
using SportsNotifierService.Model;
using SportsNotifierService.Services;
using TickerQ.Utilities.Base;

namespace SportsNotifierService.Jobs;

public class BasketballJobs(ILogger<BasketballJobs> logger, IBasketballScraper scraper, ISmtpService smtpService)
{
    [TickerFunction($"{nameof(BasketballJobs)}.{nameof(ScrapeBasketballResults)}")]
    public async Task ScrapeBasketballResults()
    {
        logger.LogDebug("Starting ScrapeBasketballResults");

        if (!await scraper.Load())
        {
            logger.LogError("ScrapeBasketballResults - Document not loaded");
            return;
        }

        var scores = scraper.GetBasketballScores();
        var message = CreateResultMessage(scores);

        await smtpService.SendEmailAsync(message);
    }

    private MimeMessage CreateResultMessage(List<MatchSummary> scores)
    {
        var message = new MimeMessage()
        {
            Subject = $"Basketball Scores - {DateTime.UtcNow:D}"
        };

        var plain = new TextPart("plain")
        {
            Text = "No Matches played today"
        };

        var writer = new StringWriter();
        var xml = new XmlTextWriter(writer)
        {
            Formatting = Formatting.Indented
        };

        if (scores.Count > 0)
            GenerateScoresTable(xml, scores);
        else
            xml.WriteElementString("p", "There were no matches played today.");

        xml.WriteStartElement("br");
        xml.WriteEndElement();
        xml.WriteElementString("p", "Kind Regards");
        xml.WriteElementString("p", "Sports Notifier Team");
        xml.Flush();

        var html = new TextPart("html")
        {
            Text = writer.ToString()
        };

        var alternative = new MultipartAlternative();
        alternative.Add(plain);
        alternative.Add(html);

        var multipart = new Multipart();
        multipart.Add(alternative);

        message.Body = multipart;

        return message;
    }

    private void GenerateScoresTable(XmlTextWriter xml, List<MatchSummary> scores)
    {
        xml.WriteStartElement("table");
        
        WriteTableHead(xml);
        WriteTableBody(xml, scores);
        WriteTableFooter(xml, scores.Count);
        
        xml.WriteEndElement();
        
    }

    private void WriteTableHead(XmlTextWriter xml)
    {
        xml.WriteStartElement("thead");
        xml.WriteStartElement("tr");
        
        xml.WriteElementString("th", "Home Team");
        xml.WriteElementString("th", "Away Team");
        xml.WriteElementString("th", "Score");
        xml.WriteElementString("th", "Final");
        
        xml.WriteEndElement();
        xml.WriteEndElement();
    }

    private void WriteTableBody(XmlTextWriter xml, List<MatchSummary> scores)
    {
        xml.WriteStartElement("tbody");
        
        foreach (var score in scores)
            WriteTableRow(xml, score);
        
        xml.WriteEndElement();
    }
    
    private void WriteTableRow(XmlTextWriter xml, MatchSummary score)
    {
        xml.WriteStartElement("tr");
            
        xml.WriteElementString("td", score.Home.TeamName);
        xml.WriteElementString("td", score.Away.TeamName);
        xml.WriteElementString("td", $"{score.Home.Score} - {score.Away.Score}");
        xml.WriteElementString("td", score.IsFinal ? "Final" : "Playing");
            
        xml.WriteEndElement();
    }

    private void WriteTableFooter(XmlTextWriter xml, int count)
    {
        xml.WriteStartElement("tfoot");
        xml.WriteStartElement("tr");
        
        xml.WriteStartElement("th");
        xml.WriteAttributeString("scope", "row");
        xml.WriteAttributeString("colspan", "3");
        xml.WriteString("Matches played today");
        xml.WriteEndElement();
        
        xml.WriteElementString("td", $"{count}");
        
        xml.WriteEndElement();
        xml.WriteEndElement();
    }
}