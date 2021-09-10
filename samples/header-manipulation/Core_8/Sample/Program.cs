using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Headers";
        var endpointConfiguration = new EndpointConfiguration("Samples.Headers");

        endpointConfiguration.UseTransport(new LearningTransport());

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
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        #region sending

        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}