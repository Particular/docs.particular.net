﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.ImmutableMessages.UsingInterfaces.Receiver");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());

        endpointConfiguration.ApplyCustomConventions();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Samples.ImmutableMessages.UsingInterfaces.Receiver started. Press any key to exit.");
        Console.ReadKey();

        await endpointInstance.Stop();
    }
}

