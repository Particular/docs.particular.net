﻿using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        var endpointConfiguration = new EndpointConfiguration("Subscriber");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        routing.RegisterPublisher(typeof(Messages.SomethingHappened), "Publisher");

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
        Console.ReadKey();

        await endpointInstance.Stop()
                .ConfigureAwait(false);
    }
}