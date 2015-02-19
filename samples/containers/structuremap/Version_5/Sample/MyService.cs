using NServiceBus.Logging;

public class MyService
{
    ILog log = LogManager.GetLogger<MyService>();
    public void WriteHello()
    {
        log.Info("Hello from MyService.");
    }
}