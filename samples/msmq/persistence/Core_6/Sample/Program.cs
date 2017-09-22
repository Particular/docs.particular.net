using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Msmq.Persistence";

        var endpointConfiguration = new EndpointConfiguration("Samples.Msmq.Persistence");
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #region ConfigureMsmqPersistenceEndpoint

        var persistence = endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();
        persistence.SubscriptionQueue("Samples.Msmq.Persistence.Subscriptions");

        endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();

        #endregion

        var routing = transport.Routing();
        routing.RegisterPublisher(typeof(MyEvent), "Samples.Msmq.Persistence");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var myMessage = new MyEvent();
        await endpointInstance.Publish(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}