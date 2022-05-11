using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Samples.Azure.ServiceBus.Bridge";

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }

        await Host.CreateDefaultBuilder()
            .UseNServiceBusBridge((ctx, bridgeConfiguration) =>
            {
                #region create-asb-endpoint-of-bridge
                var asbBridgeEndpoint = new BridgeEndpoint("Samples.Azure.ServiceBus.AsbEndpoint");
                #endregion

                #region asb-subscribe-to-event-via-bridge
                asbBridgeEndpoint.RegisterPublisher<MyEvent>("Samples.Azure.ServiceBus.MsmqEndpoint");
                #endregion

                var asbBridgeTransport = new BridgeTransport(new AzureServiceBusTransport(connectionString));
                asbBridgeTransport.AutoCreateQueues = true;
                asbBridgeTransport.HasEndpoint(asbBridgeEndpoint);
                bridgeConfiguration.AddTransport(asbBridgeTransport);

                var msmqBridgeTransport = new BridgeTransport(new MsmqTransport());
                msmqBridgeTransport.AutoCreateQueues = true;
                var msmqBridgeEndpoint = new BridgeEndpoint("Samples.Azure.ServiceBus.MsmqEndpoint", $"Samples.Azure.ServiceBus.MsmqEndpoint@{Environment.MachineName}");
                msmqBridgeEndpoint.RegisterPublisher<OtherEvent>("Samples.Azure.ServiceBus.AsbEndpoint");
                msmqBridgeTransport.HasEndpoint(msmqBridgeEndpoint);
                bridgeConfiguration.AddTransport(msmqBridgeTransport);                
            })
            .Build()
            .RunAsync().ConfigureAwait(false);
    }
}