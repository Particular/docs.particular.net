using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Msmq.Persistence";
        #region ConfigureMsmqPersistenceEndpoint

        var endpointConfiguration = new EndpointConfiguration("Samples.Msmq.Persistence");
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        #endregion
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
        endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>()
            .SubscriptionQueue("Samples.Msmq.Persistence.Subscriptions");
        
        transport.Routing().RegisterPublisher(typeof(MyEvent), "Samples.Msmq.Persistence");

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