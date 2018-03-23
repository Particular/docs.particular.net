using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MessageDurability.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.MessageDurability.Receiver");
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.UseNonTransactionalQueues();
        transport.Transactions(TransportTransactionMode.None);
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}