using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ASB.Publisher";

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Publisher");

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.Conventions().DefiningEventsAs(type => type.Name == nameof(EventTwo) || type.Name == nameof(EventOne));


        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var transport = new AzureServiceBusTransport(connectionString);
        endpointConfiguration.UseTransport(transport);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to publish events");
        Console.ReadKey();
        Console.WriteLine();

        await endpointInstance.Publish(new EventOne
        {
            Content = $"{nameof(EventOne)} sample content",
            PublishedOnUtc = DateTime.UtcNow
        });

        await endpointInstance.Publish(new EventTwo
        {
            Content = $"{nameof(EventTwo)} sample content",
            PublishedOnUtc = DateTime.UtcNow
        });

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}