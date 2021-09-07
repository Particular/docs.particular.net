using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.InstanceMappingFile.Billing";
        var endpointConfiguration = new EndpointConfiguration("Samples.InstanceMappingFile.Billing");
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var routing = endpointConfiguration.UseTransport(new MsmqTransport());
        var routingTable = routing.InstanceMappingFile();
        routingTable.FilePath(@"..\..\..\..\instance-mapping.xml");
        routing.RegisterPublisher(typeof(OrderAccepted), "Samples.InstanceMappingFile.Sales");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}