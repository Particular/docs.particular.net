using System;
using System.Threading.Tasks;
using System.Transactions;
using NServiceBus;
using NServiceBus.Bridge;
using NServiceBus.Configuration.AdvancedExtensibility;
using NServiceBus.Serialization;
using NServiceBus.Settings;

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

        // TODO: ASB requires serializer to be registered.
        // Currently, there's no way to specify serialization for the bridged endpoints
        // endpointConfiguration.UseSerialization<T>();

        #region bridge-general-configuration

        var bridgeConfiguration = Bridge
            .Between<MsmqTransport>("Bridge-MSMQ")
            .And<AzureServiceBusTransport>("Bridge-ASB", transport =>
            {
                transport.ConnectionString(connectionString);
                transport.UseForwardingTopology();
                var settings = transport.GetSettings();
                var serializer = Tuple.Create(new NewtonsoftSerializer() as SerializationDefinition, new SettingsHolder());
                settings.Set("MainSerializer", serializer);
            });

        bridgeConfiguration.AutoCreateQueues();
        bridgeConfiguration.UseSubscriptionPersistence<InMemoryPersistence>((configuration, persistence) => { });

        #endregion

        #region suppress-transaction-scope-for-asb

        // TODO: Bridge 2.x doesn't support this yet

        bridgeConfiguration.InterceptForwarding(async (queue, message, dispatch, forward) =>
        {
            using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                await forward(dispatch).ConfigureAwait(false);
            }
        });

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
