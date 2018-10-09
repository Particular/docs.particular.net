﻿using System;
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
                //Prevents ASB from using TransactionScope
                transport.Transactions(TransportTransactionMode.ReceiveOnly);
                transport.ConnectionString(connectionString);
                
                var settings = transport.GetSettings();
                var serializer = Tuple.Create(new NewtonsoftSerializer() as SerializationDefinition, new SettingsHolder());
                settings.Set("MainSerializer", serializer);

                var topology = transport.UseEndpointOrientedTopology();
                topology.RegisterPublisher(typeof(OtherEvent), "Samples.Azure.ServiceBus.AsbEndpoint");
            });

        bridgeConfiguration.AutoCreateQueues();
        bridgeConfiguration.UseSubscriptionPersistence(new InMemorySubscriptionStorage());

        #endregion

        #region resubscriber
        var resubscriber = await Resubscriber<MsmqTransport>.Create(
            inputQueueName: "Bridge-MSMQ", 
            delay: TimeSpan.FromSeconds(10), 
            configureTransport: t => { });

        bridgeConfiguration.InterceptForwarding(resubscriber.InterceptMessageForwarding);
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