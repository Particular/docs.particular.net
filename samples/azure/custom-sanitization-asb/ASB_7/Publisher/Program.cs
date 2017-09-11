using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared.Messages.In.A.Deep.Nested.Namespace.Nested.Events;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ASB.Serialization.Publisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Serialization.Publisher");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        transport.UseForwardingTopology();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: settings => { settings.NumberOfRetries(0); });
        recoverability.DisableLegacyRetriesSatellite();

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