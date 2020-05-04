using System.Threading;
using Microsoft.Extensions.Logging;

public class SharedDependency
{
    private int increment;
    private ILogger<SharedDependency> logger;

    public SharedDependency(ILogger<SharedDependency> logger)
    {
        this.logger = logger;
    }

    public void Called(string from)
    {
        var newValue = Interlocked.Increment(ref increment);
        logger.LogInformation($"Called '{@from}'. New value is '{newValue}'");
    }
}