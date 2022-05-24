using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Samples.Transport.Bridge.Bridge";

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        await Host.CreateDefaultBuilder()
            .UseNServiceBusBridge((ctx, bridgeConfiguration) =>
            {
                #region create-asb-endpoint-of-bridge
                var asbBridgeEndpoint = new BridgeEndpoint("Samples.Transport.Bridge.AsbEndpoint");
                #endregion

                #region asb-subscribe-to-event-via-bridge
                asbBridgeEndpoint.RegisterPublisher<MyEvent>("Samples.Transport.Bridge.MsmqEndpoint");
                #endregion

                #region asb-bridge-configuration
                var asbBridgeTransport = new BridgeTransport(new AzureServiceBusTransport(connectionString));
                asbBridgeTransport.AutoCreateQueues = true;
                asbBridgeTransport.HasEndpoint(asbBridgeEndpoint);
                bridgeConfiguration.AddTransport(asbBridgeTransport);
                #endregion

                #region create-msmq-endpoint-of-bridge
                var msmqBridgeEndpoint = new BridgeEndpoint("Samples.Transport.Bridge.MsmqEndpoint");
                #endregion

                #region msmq-subscribe-to-event-via-bridge
                msmqBridgeEndpoint.RegisterPublisher<OtherEvent>("Samples.Transport.Bridge.AsbEndpoint");
                #endregion

                #region msmq-bridge-configuration
                var msmqBridgeTransport = new BridgeTransport(new MsmqTransport());
                msmqBridgeTransport.AutoCreateQueues = true;
                msmqBridgeTransport.HasEndpoint(msmqBridgeEndpoint);
                bridgeConfiguration.AddTransport(msmqBridgeTransport);
                #endregion
            })
            .Build()
            .RunAsync().ConfigureAwait(false);
    }
}