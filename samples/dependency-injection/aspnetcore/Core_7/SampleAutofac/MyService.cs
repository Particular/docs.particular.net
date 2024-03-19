using Microsoft.Extensions.Logging;

public class MyService
{
    readonly ILogger logger;

    public MyService(ILogger<MyService> logger)
    {
        this.logger = logger;
    }

    public void WriteHello()
    {
        logger.LogInformation("Hello from MyService.");
    }
}