using NServiceBus.Logging;

public class MyService
{
    static ILog log = LogManager.GetLogger(typeof(MyService));

    public void WriteHello()
    {
        log.Info("Hello from MyService.");
    }
}