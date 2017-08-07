﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Sqs.Simple";
        #region ConfigureEndpoint

        var endpointConfiguration = new EndpointConfiguration("Samples.Sqs.Simple");
        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.Region("us-east-1");

        #endregion
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        #region sendonly
        //TODO: uncomment to view a message in transit
        //endpointConfiguration.SendOnly();
        #endregion
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