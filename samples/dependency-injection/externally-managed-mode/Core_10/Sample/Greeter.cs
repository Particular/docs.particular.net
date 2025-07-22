using NServiceBus.Logging;

public class Greeter
{
    private static readonly ILog log = LogManager.GetLogger<Greeter>();

    public void SayHello()
    {
        log.Info("Hello from Greeter.");
    }
}