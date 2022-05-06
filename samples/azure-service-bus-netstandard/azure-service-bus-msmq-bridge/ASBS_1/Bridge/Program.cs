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
                var asbTransport = new AzureServiceBusTransport(connectionString)
                {
                    //TransportTransactionMode = TransportTransactionMode.ReceiveOnly
                    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
                };
                var asbBridgeTransport = new BridgeTransport(asbTransport);
                asbBridgeTransport.AutoCreateQueues = true;
                var asbEndpoint = new BridgeEndpoint("Samples.Azure.ServiceBus.AsbEndpoint");
                asbEndpoint.RegisterPublisher<MyEvent>("Samples.Azure.ServiceBus.MsmqEndpoint");
                asbBridgeTransport.HasEndpoint(asbEndpoint);

                var msmqTransport = new MsmqTransport()
                {
                    //TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
                };
                var msmqBridgeTransport = new BridgeTransport(msmqTransport);
                msmqBridgeTransport.AutoCreateQueues = true;
                var msmqEndpoint = new BridgeEndpoint("Samples.Azure.ServiceBus.MsmqEndpoint");
                msmqEndpoint.RegisterPublisher<OtherEvent>("Samples.Azure.ServiceBus.AsbEndpoint");
                msmqBridgeTransport.HasEndpoint(msmqEndpoint);

                bridgeConfiguration.AddTransport(msmqBridgeTransport);
                bridgeConfiguration.AddTransport(asbBridgeTransport);
            })
            .Build()
            .RunAsync().ConfigureAwait(false);
    }
}