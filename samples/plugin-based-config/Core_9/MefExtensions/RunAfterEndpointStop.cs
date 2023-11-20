using System.Composition;
using System.Threading.Tasks;
using NServiceBus.Logging;

[Export(typeof(IRunAfterEndpointStop))]
public class RunAfterEndpointStop :
    IRunAfterEndpointStop
{
    static ILog log = LogManager.GetLogger<RunAfterEndpointStop>();

    public Task Run()
    {
        log.Info("Endpoint Stopped");
        return Task.CompletedTask;
    }
}