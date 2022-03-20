using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Red.Sales";
        var endpointConfiguration = new EndpointConfiguration("Red.Sales");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(ConnectionStrings.Red);
        transport.Transactions(TransportTransactionMode.ReceiveOnly);
        transport.TopicName("bundle-red");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}