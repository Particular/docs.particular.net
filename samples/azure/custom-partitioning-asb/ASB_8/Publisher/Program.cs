using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
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
        namespacePartitioning.UseStrategy<ReplicatedNamespacePartitioningStrategy>();

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: settings =>
            {
                settings.NumberOfRetries(0);
            });

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press 'e' to publish an event");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var eventId = Guid.NewGuid();

            if (key.Key != ConsoleKey.E)
            {
                break;
            }
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