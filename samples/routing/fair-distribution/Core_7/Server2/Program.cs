﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Server2";
        var endpointConfiguration = new EndpointConfiguration("Samples.FairDistribution.Server");
        endpointConfiguration.OverrideLocalAddress("Samples.FairDistribution.Server-2");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        endpointConfiguration.EnableFeature<FairDistribution>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}