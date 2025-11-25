using SportsNotifierService;
using TickerQ.DependencyInjection;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Interfaces.Managers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTickerQ();

var app = builder.Build();

app.UseTickerQ();

// TODO move to controller
app.MapPost("/basketball/scrapeNow", async (ITimeTickerManager<TimeTickerEntity> manager) =>
{
    await manager.AddAsync(new TimeTickerEntity
    {
        ExecutionTime = DateTime.Now.AddSeconds(10),
        Function = nameof(MyJobs.ScrapeBasketballResults),
        Description = "Scrape basketball results 'on demand'",
        Retries = 3,
        RetryIntervals = [1, 3, 10]
    });
    
    return Results.Ok();
});

app.Run();