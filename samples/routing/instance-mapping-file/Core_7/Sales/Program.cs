using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.InstanceMappingFile.Sales.1";
        var endpointConfiguration = new EndpointConfiguration("Samples.InstanceMappingFile.Sales");
        endpointConfiguration.OverrideLocalAddress("Samples.InstanceMappingFile.Sales-1");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        var routingTable = routing.InstanceMappingFile();
        routingTable.FilePath(@"..\..\..\..\instance-mapping.xml");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}