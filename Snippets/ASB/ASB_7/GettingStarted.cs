namespace Snippets6.Azure.Transports.AzureServiceBus
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.AzureServiceBus;

    class GettingStarted
    {
        async Task GettingStartedUsage()
        {
            #region AzureServiceBusTransportGettingStarted 7

            EndpointConfiguration configuration = new EndpointConfiguration("myendpoint");
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.SendFailedMessagesTo("error");
            configuration
                .UseTransport<AzureServiceBusTransport>()
                .UseTopology<ForwardingTopology>()
                .ConnectionString("Paste your connectionstring here");

            IStartableEndpoint initializableEndpoint = await Endpoint.Create(configuration);
            IEndpointInstance endpoint = await initializableEndpoint.Start();

            #endregion
        }
    }
}