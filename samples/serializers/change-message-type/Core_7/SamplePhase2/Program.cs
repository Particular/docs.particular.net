using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Phase2";

        var endpointConfiguration = new EndpointConfiguration("ChangeMessageIdentity.Phase2");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region RegisterMessageMutator

        endpointConfiguration.RegisterMessageMutator(new MessageIdentityMutator());

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}