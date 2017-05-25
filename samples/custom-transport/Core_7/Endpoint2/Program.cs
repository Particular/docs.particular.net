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
        Console.Title = "Samples.CustomTransport.Endpoint2";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomTransport.Endpoint2");
        endpointConfiguration.UseTransport<FileTransport>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.DisableFeature<TimeoutManager>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}