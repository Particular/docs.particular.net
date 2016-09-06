using System.Threading.Tasks;
using NServiceBus;

class GettingStarted
{
    async Task GettingStartedUsage()
    {
        #region AzureServiceBusTransportGettingStarted

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.UseTopology<ForwardingTopology>();
        transport.ConnectionString("Paste connectionstring here");

        var startableEndpoint = await Endpoint.Create(endpointConfiguration)
            .ConfigureAwait(false);
        var endpointInstance = await startableEndpoint.Start()
            .ConfigureAwait(false);

        #endregion
    }
}