using System.Threading.Tasks;
using NServiceBus;

#region 5to6-EndpointStartAndStopCloudHost
// Use this in NServiceBus.Host or NServiceBus.Host.AzureCloudService
public class Bootstrapper : IWantToRunWhenEndpointStartsAndStops
{
    public Task Start(IMessageSession session)
    {
        // Do startup actions here.
        // Either mark Start method as async or do the following
        return Task.FromResult(0);
    }

    public Task Stop(IMessageSession session)
    {
        // Do cleanup actions here.
        // Either mark Stop method as async or do the following
        return Task.FromResult(0);
    }
}
#endregion
