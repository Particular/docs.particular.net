﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.RabbitMQ.Simple";
        #region ConfigureRabbit
        var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.Simple");
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseConventionalRoutingTopology();
        transport.ConnectionString("host=localhost");
        #endregion
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}