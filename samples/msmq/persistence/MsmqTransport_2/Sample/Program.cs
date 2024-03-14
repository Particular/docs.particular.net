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
        var transport = new MsmqTransport
        {
            TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
        };
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        var routing = endpointConfiguration.UseTransport(transport);
        routing.RegisterPublisher(typeof(MyEvent), "Samples.Msmq.Persistence");

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #region ConfigureMsmqPersistenceEndpoint

        endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();
        // disable delayed retries as MSMQ doesn't support timeouts natively:
        endpointConfiguration.Recoverability().Delayed(settings => settings.NumberOfRetries(0));

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to publish");
        Console.ReadKey();

        var myMessage = new MyEvent();
        await endpointInstance.Publish(myMessage);

        Console.WriteLine();
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop();
    }
}
