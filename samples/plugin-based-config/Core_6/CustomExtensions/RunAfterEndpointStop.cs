using System.Threading.Tasks;
using NServiceBus.Logging;

public class RunAfterEndpointStop :
    IRunAfterEndpointStop
{
    static ILog log = LogManager.GetLogger<RunAfterEndpointStop>();

    public Task Run()
    {
        log.Info("Endpoint Stopped");
        return Task.FromResult(0);
    }
}