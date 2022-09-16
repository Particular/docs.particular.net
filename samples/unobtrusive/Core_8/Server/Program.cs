﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Unobtrusive.Server";
        var endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Server");
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>()
            .BasePath(@"..\..\..\..\DataBusShare\");

        endpointConfiguration.ApplyCustomConventions();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await CommandSender.Start(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}

