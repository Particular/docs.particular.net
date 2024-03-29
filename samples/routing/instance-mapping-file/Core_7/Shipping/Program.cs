﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Shipping";
        var endpointConfiguration = new EndpointConfiguration("Samples.InstanceMappingFile.Shipping");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        var routingTable = routing.InstanceMappingFile();
        routingTable.FilePath(@"..\..\..\..\instance-mapping.xml");
        routing.RegisterPublisher(typeof(OrderAccepted), "Samples.InstanceMappingFile.Sales");

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}