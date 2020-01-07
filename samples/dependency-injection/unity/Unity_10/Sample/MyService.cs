using NServiceBus.Logging;

public class MyService
{
    static ILog log = LogManager.GetLogger<MyService>();

    public void WriteHello()
    {
        log.Info("Hello from MyService.");
    }
}

public class MyOtherService
{
    static ILog log = LogManager.GetLogger<MyService>();

    public void WriteHello()
    {
        log.Info("Hello from MyService.");
    }
}