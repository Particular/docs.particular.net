using NServiceBus;
using NServiceBus.Logging;

#region CustomRunAfterEndpointStart
public class RunAfterEndpointStart :
    IRunAfterEndpointStart
{
    static ILog log = LogManager.GetLogger<RunAfterEndpointStart>();
    public void Run(IBus bus)
    {
        log.Info("Endpoint Started.");
    }
}
#endregion