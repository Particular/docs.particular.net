namespace Bridge_1
{
    using Messages;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using System.Threading.Tasks;

    public class Configuration
    {
        public async Task GenericHost()
        {
            #region generic-host

            await Host.CreateDefaultBuilder()
                .UseNServiceBusBridge(bridgeConfiguration =>
                {
                    // Configure the bridge
                })
                .Build()
                .RunAsync();

            #endregion
        }

        public async Task GenericHostBuilderContext()
        {
            #region generic-host-builder-context

            await Host.CreateDefaultBuilder()
                .UseNServiceBusBridge((hostBuilderContext, bridgeConfiguration) =>
                {
                    var connectionString = hostBuilderContext.Configuration.GetValue<string>("MyBridge:AzureServiceBusConnectionString");
                    var concurrency = hostBuilderContext.Configuration.GetValue<int>("MyBridge:Concurrency");

                    var transport = new BridgeTransport(new AzureServiceBusTransport(connectionString))
                    {
                        Concurrency = concurrency
                    };

                    bridgeConfiguration.AddTransport(transport);

                    // more configuration...
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
            invoicing.RegisterPublisher<OrderShipped>("Shipping");

            #endregion
        }

        public void AutoCreateQueues()
        {
            #region auto-create-queues

            var msmq = new BridgeTransport(new MsmqTransport())
            {
                AutoCreateQueues = true
            };

            var azureServiceBus = new BridgeTransport(new AzureServiceBusTransport(connectionString))
            {
                AutoCreateQueues = true
            };

            #endregion
        }

        public void CustomConcurrency()
        {
            #region custom-concurrency

            var msmq = new BridgeTransport(new MsmqTransport())
            {
                Concurrency = 10
            };

            var azureServiceBus = new BridgeTransport(new AzureServiceBusTransport(connectionString))
            {
                Concurrency = 5
            };

            #endregion
        }

        public void CustomErrorQueue()
        {
            #region custom-error-queue

            var msmq = new BridgeTransport(new MsmqTransport())
            {
                ErrorQueue = "my-msmq-bridge-error-queue"
            };

            var azureServiceBus = new BridgeTransport(new AzureServiceBusTransport(connectionString))
            {
                ErrorQueue = "my-asb-bridge-error-queue"
            };

            #endregion
        }

        public void CustomTransportName()
        {
            #region custom-transport-name

            var azureServiceBus1 = new BridgeTransport(new AzureServiceBusTransport(connectionStringNamepace1))
            {
                Name = "asb-namespace-1"
            };

            var azureServiceBus2 = new BridgeTransport(new AzureServiceBusTransport(connectionStringNamepace2))
            {
                Name = "asb-namespace-2"
            };

            #endregion
        }

        public void PlatformBridging()
        {
            #region platform-bridging

            var transportWhereServiceControlIsInstalled = new BridgeTransport(new MsmqTransport());

            transportWhereServiceControlIsInstalled.HasEndpoint("Particular.ServiceControl");
            transportWhereServiceControlIsInstalled.HasEndpoint("Particular.Monitoring");
            transportWhereServiceControlIsInstalled.HasEndpoint("error");
            transportWhereServiceControlIsInstalled.HasEndpoint("audit");

            #endregion
        }

        public void QueueName()
        {
            #region custom-address

            var transport = new BridgeTransport(new MsmqTransport());
            transport.HasEndpoint("Finance", "finance@machinename");

            var endpoint = new BridgeEndpoint("Sales", "sales@another-machine");
            transport.HasEndpoint(endpoint);

            #endregion
        }

        string connectionString = string.Empty;
        string connectionStringNamepace1 = string.Empty;
        string connectionStringNamepace2 = string.Empty;
    }
}