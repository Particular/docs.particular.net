﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Router.MixedTransports.Server";
        var endpointConfiguration = new EndpointConfiguration(
            "Samples.Router.MixedTransports.Server");

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=localhost");
        transport.UseConventionalRoutingTopology();

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();

        await endpointInstance.Stop();
    }
}