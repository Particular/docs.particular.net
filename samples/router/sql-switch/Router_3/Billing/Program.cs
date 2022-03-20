using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Green.Billing";
        var endpointConfiguration = new EndpointConfiguration("Green.Billing");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(ConnectionStrings.Red);
        transport.Transactions(TransportTransactionMode.ReceiveOnly);
        transport.TopicName("bundle-green");

        #region BillingRouterConfig

        var routing = transport.Routing();
        var router = routing.ConnectToRouter("Switch-Green");
        router.RegisterPublisher(typeof(OrderAccepted), "Red.Sales");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}