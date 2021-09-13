using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Subscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.ManualUnsubscribe.Subscriber");

        var routing = endpointConfiguration.UseTransport(new MsmqTransport());
        routing.RegisterPublisher(typeof(SomethingHappened), "Samples.ManualUnsubscribe.Publisher");

        endpointConfiguration.UsePersistence<NonDurablePersistence>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}