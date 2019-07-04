using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

class Program
{
    static async Task Main()
    {
        var endpointName = "Samples.Msmq.Persistence";

        Console.Title = "Samples.Msmq.Persistence";

        var endpointConfiguration = new EndpointConfiguration(endpointName);
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #region ConfigureMsmqPersistenceEndpoint

        var persistence = endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();

        //Default queue name, `NServiceBus.Subscriptions`, is overridden to avoid sharing the subscription storage queue with other endpoints on the same machine. 
        persistence.SubscriptionQueue($"{endpointName}.v6Subscriptions");

        endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();

        #endregion

        var routing = transport.Routing();
        routing.RegisterPublisher(typeof(MyEvent), endpointName);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to publish");
        Console.ReadKey();

        var myMessage = new MyEvent();
        await endpointInstance.Publish(myMessage)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}