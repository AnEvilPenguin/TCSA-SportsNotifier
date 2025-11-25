using SportsNotifierService.Services;
using TickerQ.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTickerQ();
builder.Services.AddControllers();

builder.Services.AddScoped<IBasketballScraper, BasketballScraper>();

var app = builder.Build();

app.UseTickerQ();
app.MapControllers();

app.Run();