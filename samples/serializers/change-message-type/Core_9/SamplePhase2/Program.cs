using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

static class Program
{
    static async Task Main()
    {
        Console.Title = "ChangeMessageIdentity.Phase2";

        var endpointConfiguration = new EndpointConfiguration("ChangeMessageIdentity.Phase2");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        #region RegisterMessageMutator

        endpointConfiguration.RegisterMessageMutator(new MessageIdentityMutator());

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Waiting for orders..");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
