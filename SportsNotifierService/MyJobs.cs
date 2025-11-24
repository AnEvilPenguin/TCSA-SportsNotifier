using TickerQ.Utilities.Base;

namespace SportsNotifierService;

public class MyJobs
{
    private readonly ILogger<MyJobs> _logger;

    public MyJobs(ILogger<MyJobs> logger)
    {
        _logger = logger;
    }

    [TickerFunction("CleanUpLogs", "0 */1 * * * *")]
    public void CleanUpLogs()
    {
        _logger.LogInformation("Cleaning up Logs");
    }
}