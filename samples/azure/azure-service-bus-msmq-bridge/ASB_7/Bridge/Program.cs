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
                transport.UseForwardingTopology();
            });

        bridgeConfiguration.AutoCreateQueues();
        bridgeConfiguration.UseSubscriptionPersistece<InMemoryPersistence>((configuration, persistence) => { });

        #endregion

        #region bridge-execution

        var bridge = bridgeConfiguration.Create();

        await bridge.Start().ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await bridge.Stop().ConfigureAwait(false);


        #endregion
    }
}
