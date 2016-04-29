namespace ASB_7
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.AzureServiceBus;

    class GettingStarted
    {
        async Task GettingStartedUsage()
        {
            #region AzureServiceBusTransportGettingStarted 7

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("myendpoint");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            transport.UseTopology<ForwardingTopology>();
            transport.ConnectionString("Paste connectionstring here");

            IStartableEndpoint initializableEndpoint = await Endpoint.Create(endpointConfiguration);
            IEndpointInstance endpoint = await initializableEndpoint.Start();

            #endregion
        }
    }
}