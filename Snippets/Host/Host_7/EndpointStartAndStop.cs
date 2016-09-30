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
        return Task.CompletedTask;
    }

    public Task Stop(IMessageSession session)
    {
        // perform shutdown logic
        return Task.CompletedTask;
    }
}

#endregion