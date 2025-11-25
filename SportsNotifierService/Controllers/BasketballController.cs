using Microsoft.AspNetCore.Mvc;
using SportsNotifierService.Jobs;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Interfaces.Managers;

namespace SportsNotifierService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketballController (ITimeTickerManager<TimeTickerEntity> manager) : ControllerBase
{
    [HttpPost("ScrapeNow")]
    public async Task<ActionResult> ScrapeNow()
    {
        var executionTime = DateTime.UtcNow.AddSeconds(10);
        
        await manager.AddAsync(new TimeTickerEntity
        {
            ExecutionTime = executionTime,
            Function = $"{nameof(BasketballJobs)}.{nameof(BasketballJobs.ScrapeBasketballResults)}",
            Description = "Scrape basketball results 'on demand'",
            Retries = 3,
            RetryIntervals = [1, 3, 10]
        });
    
        return Ok(executionTime);
    }
}