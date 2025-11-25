using SportsNotifierService.Jobs;
using SportsNotifierService.Model;
using SportsNotifierService.Services;
using TickerQ.DependencyInjection;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Interfaces.Managers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTickerQ();
builder.Services.AddControllers();

builder.Services.AddScoped<IBasketballScraper, BasketballScraper>();

builder.Services.AddScoped<ISmtpService, SmtpService>();
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SMTP"));

var cronExpression = builder.Configuration.GetSection("Schedule")?.GetValue<string>("CronExpression");

var app = builder.Build();

app.UseTickerQ();
app.MapControllers();

if (!string.IsNullOrEmpty(cronExpression))
{
    var manager = app.Services.GetService<ICronTickerManager<CronTickerEntity>>();

    if (manager != null)
        await manager.AddAsync(new CronTickerEntity()
        {
            Expression = cronExpression,
            Function = $"{nameof(BasketballJobs)}.{nameof(BasketballJobs.ScrapeBasketballResults)}",
            Description = "Scrape basketball results on a schedule",
            Retries = 3,
            RetryIntervals = [1, 3, 10]
        });
}

app.Run();