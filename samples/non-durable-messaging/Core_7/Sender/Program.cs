using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MessageDurability.Sender";
        
        var endpointConfiguration = new EndpointConfiguration("Samples.MessageDurability.Sender");

        // startcode useTransactionalQueues-false
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.UseNonTransactionalQueues();
        // endcode

        transport.Transactions(TransportTransactionMode.None);

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await endpointInstance.Send("Samples.MessageDurability.Receiver", new MyMessage())
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}