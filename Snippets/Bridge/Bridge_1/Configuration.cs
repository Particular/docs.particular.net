namespace Bridge_1
{
    using Messages;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using System.Threading.Tasks;

    public class Configuration
    {
        public async Task GenericHost()
        {
            #region generic-host

            await Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .UseNServiceBusBridge((ctx, bridgeConfiguration) =>
                {
                    // Configure the bridge
                })
                .Build()
                .RunAsync().ConfigureAwait(false);

            #endregion
        }

        public async Task EndpointRegistration()
        {
            #region endpoint-registration

            await Host.CreateDefaultBuilder()
                .UseNServiceBusBridge((ctx, bridgeConfiguration) =>
                {
                    var msmq = new BridgeTransport(new MsmqTransport());
                    msmq.HasEndpoint("Sales");
                    msmq.HasEndpoint("Shipping");

                    var asb = new BridgeTransport(new AzureServiceBusTransport(connectionString));
                    asb.HasEndpoint("Finance.Invoicing");
                    asb.HasEndpoint("Finance.Billing");

                    bridgeConfiguration.AddTransport(msmq);
                    bridgeConfiguration.AddTransport(asb);
                })
                .Build()
                .RunAsync().ConfigureAwait(false);

            #endregion
        }

        public void RegisterPublishers()
        {
            #region register-publisher

            var msmq = new BridgeTransport(new MsmqTransport());
            var asb = new BridgeTransport(new AzureServiceBusTransport(connectionString));

            msmq.HasEndpoint("Sales");
            msmq.HasEndpoint("Finance.Billing");

            var invoicing = new BridgeEndpoint("Finance.Invoicing");
            invoicing.RegisterPublisher("Messages.OrderPlaced", "Sales");
            invoicing.RegisterPublisher(typeof(OrderBilled), "Finance.Billing");

            #endregion
        }

        string connectionString = string.Empty;
    }
}