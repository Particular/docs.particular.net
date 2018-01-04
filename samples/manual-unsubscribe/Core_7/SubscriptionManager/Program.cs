using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "SubscriptionManager";

        #region SubscriptionManager-config

        Console.WriteLine("Press any key to unsubscribe 'Subscriber' from 'Publisher'");
        Console.ReadKey();
        var endpointConfiguration = new EndpointConfiguration("Samples.ManualUnsubscribe.SubscriptionManager");
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendOnly();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var typeToUnscubscribe = typeof(SomethingHappened);
        var unsubscribeMessage = new ManualUnsubscribe
        {
            MessageTypeName = typeToUnscubscribe.AssemblyQualifiedName,
            SubscriberEndpoint = "Samples.ManualUnsubscribe.Subscriber"
        };
        await endpointInstance.Send("Samples.ManualUnsubscribe.Publisher", unsubscribeMessage)
            .ConfigureAwait(false);

        await endpointInstance.Stop()
            .ConfigureAwait(false);
        Console.WriteLine("Unsubscribe message sent. Press any other key to exit");
        Console.ReadKey();

        #endregion
    }
}