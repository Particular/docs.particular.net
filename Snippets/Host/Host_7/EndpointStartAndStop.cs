using System.Threading.Tasks;
using NServiceBus;

#region lifecycle-EndpointStartAndStopHost

// When using NServiceBus.Host or NService.Host.AzureCloudService
class RunWhenEndpointStartsAndStops :
    IWantToRunWhenEndpointStartsAndStops
{
    public Task Start(IMessageSession session)
    {
        // perform startup logic
        return Task.FromResult(0);
    }

    public Task Stop(IMessageSession session)
    {
        // perform shutdown logic
        return Task.FromResult(0);
    }
}

#endregion