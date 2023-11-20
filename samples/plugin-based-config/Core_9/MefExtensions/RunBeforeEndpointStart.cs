using System.Composition;
using System.Threading.Tasks;
using NServiceBus.Logging;

[Export(typeof(IRunBeforeEndpointStart))]
public class RunBeforeEndpointStart :
    IRunBeforeEndpointStart
{
    static ILog log = LogManager.GetLogger<RunBeforeEndpointStart>();

    public Task Run()
    {
        log.Info("Endpoint Starting");
        return Task.CompletedTask;
    }
}