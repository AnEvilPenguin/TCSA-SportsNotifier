using SportsNotifierService.Model;
using SportsNotifierService.Services;
using TickerQ.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTickerQ();
builder.Services.AddControllers();

builder.Services.AddScoped<IBasketballScraper, BasketballScraper>();

builder.Services.AddScoped<ISmtpService, SmtpService>();
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SMTP"));

var app = builder.Build();

app.UseTickerQ();
app.MapControllers();

app.Run();