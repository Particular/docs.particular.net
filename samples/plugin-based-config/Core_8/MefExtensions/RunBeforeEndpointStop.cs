using System.Composition;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

[Export(typeof(IRunBeforeEndpointStop))]
public class RunBeforeEndpointStop :
    IRunBeforeEndpointStop
{
    static ILog log = LogManager.GetLogger<RunBeforeEndpointStop>();

    public Task Run(IEndpointInstance endpoint)
    {
        log.Info("Endpoint Stopping");
        return Task.CompletedTask;
    }
}