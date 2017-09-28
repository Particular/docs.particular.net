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

        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.RegisterMessageMutator(new MutateIncomingMessages());
        endpointConfiguration.RegisterMessageMutator(new MutateIncomingTransportMessages());
        endpointConfiguration.RegisterMessageMutator(new MutateOutgoingMessages());
        endpointConfiguration.RegisterMessageMutator(new MutateOutgoingTransportMessages());

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