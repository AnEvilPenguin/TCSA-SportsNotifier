namespace SportsNotifierService.Services;

public interface IBasketballScraper
{
    public Task<bool> Load();
}