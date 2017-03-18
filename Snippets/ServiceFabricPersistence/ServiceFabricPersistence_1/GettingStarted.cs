using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.ServiceFabric;

class GettingStarted
{
    async Task GettingStartedUsage()
    {
        #region ServiceFabricPersistenceConfiguration

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.UsePersistence<ServiceFabricPersistence>();

        var startableEndpoint = await Endpoint.Create(endpointConfiguration)
            .ConfigureAwait(false);
        var endpointInstance = await startableEndpoint.Start()
            .ConfigureAwait(false);

        #endregion
    }
}