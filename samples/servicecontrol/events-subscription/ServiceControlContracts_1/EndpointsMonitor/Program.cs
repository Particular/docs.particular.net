﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "EndpointsMonitor";
        var endpointConfiguration = new EndpointConfiguration("EndpointsMonitor");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.Routing().RegisterPublisher(
            typeof(ServiceControl.Contracts.MessageFailed).Assembly,
            "Particular.ServiceControl"
        );

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningEventsAs(
            type =>
            {
                return typeof(IEvent).IsAssignableFrom(type) ||
                       // include ServiceControl events
                       type.Namespace != null &&
                       type.Namespace.StartsWith("ServiceControl.Contracts");
            });


        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to finish.");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}