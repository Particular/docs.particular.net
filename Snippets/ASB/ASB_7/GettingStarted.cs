using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.AzureServiceBus;

class GettingStarted
{
    async Task GettingStartedUsage()
    {
        #region AzureServiceBusTransportGettingStarted 7

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.UseTopology<ForwardingTopology>();
        transport.ConnectionString("Paste connectionstring here");

        var initializableEndpoint = await Endpoint.Create(endpointConfiguration)
            .ConfigureAwait(false);
        var endpointInstance = await initializableEndpoint.Start()
            .ConfigureAwait(false);

        #endregion
    }
}