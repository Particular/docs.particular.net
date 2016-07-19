using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class RunBeforeEndpointStop :
    IRunBeforeEndpointStop
{
    static ILog log = LogManager.GetLogger<RunBeforeEndpointStop>();

    public Task Run(IEndpointInstance endpoint)
    {
        log.Info("Endpoint Stopping");
        return Task.FromResult(0);
    }
}