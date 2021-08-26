using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Versioning.V2.Subscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.Versioning.V2.Subscriber");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        #region V2SubscriberMapping

        var routing = transport.Routing();
        routing.RegisterPublisher(
            assembly: typeof(ISomethingHappened).Assembly,
            publisherEndpoint: "Samples.Versioning.V2.Publisher");
        routing.RegisterPublisher(
            assembly: typeof(ISomethingMoreHappened).Assembly,
            publisherEndpoint: "Samples.Versioning.V2.Publisher");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}