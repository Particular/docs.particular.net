using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Bridge.MixedTransports.Bridge";

        #region BridgeConfig

        var bridgeConfig = Bridge
            .Between<MsmqTransport>("Samples.Bridge.MixedTransports.BridgeLeft")
            .And<RabbitMQTransport>("Samples.Bridge.MixedTransports.BridgeRight", t =>
            {
                t.ConnectionString("host=localhost");
                t.UseConventionalRoutingTopology();
            });

        bridgeConfig.AutoCreateQueues();
        bridgeConfig.UseSubscriptionPersistence<InMemoryPersistence>((config, persistence) => { });

        #endregion

        var bridge = bridgeConfig.Create();

        await bridge.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await bridge.Stop().ConfigureAwait(false);
    }
}