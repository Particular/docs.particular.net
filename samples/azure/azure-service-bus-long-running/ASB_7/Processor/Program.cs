using System;
using System.Threading;
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
        Console.Title = "Samples.Azure.ServiceBus.Processor";

        var endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.Processor");
        endpointConfiguration.SendOnly();
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        transport.UseTopology<ForwardingTopology>();
        endpointConfiguration.UseSerialization<JsonSerializer>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

        var cts = new CancellationTokenSource();
        var processor = new Processor();

        processor.Start(endpointInstance, cts.Token);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        cts.Cancel();

        await endpointInstance.Stop().ConfigureAwait(false);
    }
}