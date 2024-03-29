﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Client";
        var endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Client");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus>();
        dataBus.BasePath(@"..\..\..\..\DataBusShare\");

        endpointConfiguration.ApplyCustomConventions();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await CommandSender.Start(endpointInstance);
        await endpointInstance.Stop();
    }
}

