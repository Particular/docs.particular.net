using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ASBS.HierarchyMigration.Endpoint1";

        #region config

        var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.HierarchyMigration.Endpoint1");
        endpointConfiguration.EnableInstallers();

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var transport = new AzureServiceBusTransport(connectionString);
        endpointConfiguration.UseTransport(transport);

        transport.Topology = TopicTopology.Single("bundle-to-publish-to");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press 'enter' to send a message");
        long eventCounter = 1;

        while (true)
        {
            var event1 = new Event1
            {
                Property = $"({eventCounter}) Hello from Endpoint1",
                EventNumber = eventCounter
            };
            await endpointInstance.Publish(event1);
            Console.WriteLine($"({eventCounter}) Event sent");
            eventCounter++;
            await Task.Delay(500);
        }
    }
}
