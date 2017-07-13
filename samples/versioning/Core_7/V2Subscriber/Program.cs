using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Versioning.V2Subscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.Versioning.V2Subscriber");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        #region V2SubscriberMapping

        var routing = transport.Routing();
        routing.RegisterPublisher(
            assembly: typeof(V2.Messages.ISomethingHappened).Assembly,
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