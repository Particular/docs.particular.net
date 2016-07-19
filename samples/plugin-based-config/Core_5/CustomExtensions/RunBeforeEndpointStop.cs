using NServiceBus;
using NServiceBus.Logging;

public class RunBeforeEndpointStop :
    IRunBeforeEndpointStop
{
    static ILog log = LogManager.GetLogger<RunBeforeEndpointStop>();

    public void Run(IBus bus)
    {
        log.Info("Endpoint Stopping");
    }
}