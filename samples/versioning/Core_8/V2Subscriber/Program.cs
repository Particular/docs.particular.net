using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Versioning.V2Subscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.Versioning.V2Subscriber");
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        var transport = endpointConfiguration.UseTransport(new MsmqTransport());
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #region V2SubscriberMapping

        transport.RegisterPublisher(
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