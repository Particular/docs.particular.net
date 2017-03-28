﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Gateway.RemoteSite";
        var endpointConfiguration = new EndpointConfiguration("Samples.Gateway.RemoteSite");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableFeature<Gateway>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}

