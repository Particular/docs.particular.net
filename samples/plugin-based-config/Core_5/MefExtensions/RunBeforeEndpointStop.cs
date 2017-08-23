using System.Composition;
using NServiceBus;
using NServiceBus.Logging;

[Export(typeof(IRunBeforeEndpointStop))]
public class RunBeforeEndpointStop :
    IRunBeforeEndpointStop
{
    static ILog log = LogManager.GetLogger<RunBeforeEndpointStop>();

    public void Run(IBus bus)
    {
        log.Info("Endpoint Stopping");
    }
}