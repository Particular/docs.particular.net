using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Versioning.V1Subscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.Versioning.V1Subscriber");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region V1SubscriberMapping

        var routing = transport.Routing();
        routing.RegisterPublisher(
            assembly: typeof(V1.Messages.ISomethingHappened).Assembly,
            publisherEndpoint: "Samples.Versioning.V2Publisher");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}