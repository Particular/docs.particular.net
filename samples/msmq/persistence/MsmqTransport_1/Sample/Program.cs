using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        var endpointName = "Samples.Msmq.Persistence";
        Console.Title = endpointName;

        var endpointConfiguration = new EndpointConfiguration(endpointName);
        var transport = endpointConfiguration.UseTransport<MsmqTransport>()
            .Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #region ConfigureMsmqPersistenceEndpoint

        endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
        endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();
        
        #endregion

        var routing = transport.Routing();
        routing.RegisterPublisher(typeof(MyEvent), "Samples.Msmq.Persistence");

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