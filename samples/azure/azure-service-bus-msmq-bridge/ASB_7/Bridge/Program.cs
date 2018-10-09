using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Samples.Azure.ServiceBus.Bridge";

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }

        #region bridge-general-configuration

        var bridgeConfiguration = Bridge
            .Between<MsmqTransport>("Bridge-MSMQ")
            .And<AzureServiceBusTransport>("Bridge-ASB", transport =>
            {
                //Prevents ASB from using TransactionScope
                transport.Transactions(TransportTransactionMode.ReceiveOnly);
                transport.ConnectionString(connectionString);
                var topology = transport.UseEndpointOrientedTopology();
                topology.RegisterPublisher(typeof(OtherEvent), "Samples.Azure.ServiceBus.AsbEndpoint");
            });

        bridgeConfiguration.AutoCreateQueues();
        bridgeConfiguration.UseSubscriptionPersistece<InMemoryPersistence>((configuration, persistence) => { });
        bridgeConfiguration.TypeGenerator.RegisterKnownType(typeof(OtherEvent));

        #endregion

        #region resubscriber
        var resubscriber = await Resubscriber<MsmqTransport>.Create(
            inputQueueName: "Bridge-MSMQ", 
            delay: TimeSpan.FromSeconds(10), 
            configureTransport: t => { });

        bridgeConfiguration.InterceptForawrding(resubscriber.InterceptMessageForwarding);
        #endregion

        #region bridge-execution

        var bridge = bridgeConfiguration.Create();

        await bridge.Start().ConfigureAwait(false);
        await resubscriber.Start().ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await resubscriber.Stop().ConfigureAwait(false);
        await bridge.Stop().ConfigureAwait(false);


        #endregion
    }
}
