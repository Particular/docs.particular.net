using System.Threading.Tasks;
using NServiceBus.Logging;

public class RunBeforeEndpointStart :
    IRunBeforeEndpointStart
{
    static ILog log = LogManager.GetLogger<RunBeforeEndpointStart>();

    public Task Run()
    {
        log.Info("Endpoint Starting");
        return Task.FromResult(0);
    }
}