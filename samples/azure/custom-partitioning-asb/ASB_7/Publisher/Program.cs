using System;
using System.Threading.Tasks;
using NServiceBus;
using Publisher;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ASB.Partitioning.Publisher";

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Partitioning.Publisher");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString1 = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString1");
        if (string.IsNullOrWhiteSpace(connectionString1))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString1' environment variable. Check the sample prerequisites.");
        }
        var connectionString2 = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString2");
        if (string.IsNullOrWhiteSpace(connectionString2))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString2' environment variable. Check the sample prerequisites.");
        }
        transport.UseForwardingTopology();

        #region CustomPartitioning

        var namespacePartitioning = transport.NamespacePartitioning();
        namespacePartitioning.AddNamespace("namespace1", connectionString1);
        namespacePartitioning.AddNamespace("namespace2", connectionString2);

        #endregion

        Console.WriteLine("Default strategy is DataDistributionPartitioningStrategy.");
        Console.WriteLine("Press 'c' to change to RoundRobinWithFailoverPartitioningStrategy.");
        var usingStrategyWithFailover = false;

        var strategyChoice = Console.ReadKey();
        if (strategyChoice.Key == ConsoleKey.C)
        {
            usingStrategyWithFailover = true;

            #region CustomPartitioning_RoundRobinWithFailoverStrategy

            namespacePartitioning.UseStrategy<RoundRobinWithFailoverPartitioningStrategy>();

            #endregion
        }
        else
        {
            #region CustomPartitioning_DataDistributionStrategy

            namespacePartitioning.UseStrategy<DataDistributionPartitioningStrategy>();

            #endregion
        }

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: settings => { settings.NumberOfRetries(0); });
        recoverability.DisableLegacyRetriesSatellite();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press 'e' to publish an event");
        if (usingStrategyWithFailover)
        {
            Console.WriteLine("Press 'f' to emulate 'AzureServiceBus.ConnectionString1' failure.");
        }
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.F)
            {

            }

            if (key.Key != ConsoleKey.E)
            {
                break;
            }

            var eventId = Guid.NewGuid();
            var someEvent = new SomeEvent
            {
                EventId = eventId
            };
            await endpointInstance.Publish(someEvent)
                .ConfigureAwait(false);
            Console.WriteLine($"SomeEvent sent. EventId: {eventId}");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}