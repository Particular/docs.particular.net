public class MyService(ILogger<MyService> logger)
{
    public void WriteHello()
    {
        logger.LogInformation("Hello from MyService.");
    }
}