﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MessagingBridge.MsmqEndpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.MessagingBridge.MsmqEndpoint");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<NonDurablePersistence>();

        var routingConfig = endpointConfiguration.UseTransport(new MsmqTransport());
        routingConfig.RegisterPublisher(typeof(OtherEvent), "Samples.MessagingBridge.AsbEndpoint");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
