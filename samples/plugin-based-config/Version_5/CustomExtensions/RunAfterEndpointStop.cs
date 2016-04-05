using NServiceBus.Logging;

public class RunAfterEndpointStop : IRunAfterEndpointStop
{
    static ILog log = LogManager.GetLogger<RunAfterEndpointStop>();

    public void Run()
    {
        log.Info("Endpoint Stopped");
    }
}