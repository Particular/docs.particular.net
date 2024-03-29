﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

class Program
{
    static async Task Main()
    {
        Console.Title = "Headers";
        var endpointConfiguration = new EndpointConfiguration("Samples.Headers");

        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.RegisterMessageMutator(new MutateIncomingMessages());
        endpointConfiguration.RegisterMessageMutator(new MutateIncomingTransportMessages());
        endpointConfiguration.RegisterMessageMutator(new MutateOutgoingMessages());
        endpointConfiguration.RegisterMessageMutator(new MutateOutgoingTransportMessages());

        #region pipeline-config

        endpointConfiguration.Pipeline.Register(typeof(IncomingHeaderBehavior), "Manipulates incoming headers");
        endpointConfiguration.Pipeline.Register(typeof(OutgoingHeaderBehavior), "Manipulates outgoing headers");

        #endregion

        #region global-all-outgoing

        endpointConfiguration.AddHeaderToAllOutgoingMessages("AllOutgoing", "ValueAllOutgoing");

        #endregion
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        #region sending

        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage);

        #endregion

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}