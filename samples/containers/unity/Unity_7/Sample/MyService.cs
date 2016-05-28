using NServiceBus.Logging;

public class MyService
{
    static ILog logger = LogManager.GetLogger<MyService>();

    public void WriteHello()
    {
        logger.Info("Hello from MyService.");
    }
}