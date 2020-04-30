using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Azure.ServiceBus.Processor";
        var endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.Processor");
        endpointConfiguration.SendOnly();
#pragma warning disable 618
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
#pragma warning restore 618
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        transport.UseForwardingTopology();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var processor = new Processor();

        await processor.Start(endpointInstance)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await processor.Stop()
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}