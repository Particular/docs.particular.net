using System;
using System.Threading.Tasks;
using Events;
using NServiceBus;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Polymorphic.Publisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Polymorphic.Publisher");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.UseEndpointOrientedTopology();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: settings =>
            {
                settings.NumberOfRetries(0);
            });
        recoverability.DisableLegacyRetriesSatellite();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press '1' to publish the base event");
        Console.WriteLine("Press '2' to publish the derived event");
        Console.WriteLine("Press any other key to exit");

        await Run(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task Run(IEndpointInstance endpointInstance)
    {
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var eventId = Guid.NewGuid();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    await endpointInstance.Publish<BaseEvent>(e => { e.EventId = eventId; })
                        .ConfigureAwait(false);
                    Console.WriteLine($"BaseEvent sent. EventId: {eventId}");
                    break;

                case ConsoleKey.D2:
                    await endpointInstance.Publish<DerivedEvent>(e =>
                        {
                            e.EventId = eventId;
                            e.Data = "more data";
                        })
                        .ConfigureAwait(false);
                    Console.WriteLine($"DerivedEvent sent. EventId: {eventId}");
                    break;

                default:
                    return;
            }
        }
    }
}