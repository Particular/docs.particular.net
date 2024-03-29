﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "MsmqSimple";
        #region ConfigureMsmqEndpoint

        var endpointConfiguration = new EndpointConfiguration("Samples.Msmq.Simple");
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        #endregion
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}