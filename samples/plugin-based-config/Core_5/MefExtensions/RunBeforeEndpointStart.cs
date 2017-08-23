using System.Composition;
using NServiceBus.Logging;

[Export(typeof(IRunBeforeEndpointStart))]
public class RunBeforeEndpointStart :
    IRunBeforeEndpointStart
{
    static ILog log = LogManager.GetLogger<RunBeforeEndpointStart>();

    public void Run()
    {
        log.Info("Endpoint Starting");
    }
}