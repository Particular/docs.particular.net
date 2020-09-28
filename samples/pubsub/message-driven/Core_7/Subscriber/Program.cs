using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PubSub.MessageDrivenSubscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.MessageDrivenSubscriber");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        #region SubscriptionConfiguration
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.Routing().RegisterPublisher(typeof(OrderReceived), "Samples.PubSub.MessageDrivenPublisher");
        #endregion

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}