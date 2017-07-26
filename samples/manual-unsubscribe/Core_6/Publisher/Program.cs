using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;
using System;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.Title = "Publisher";
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        #region publisher-config
        var endpointConfiguration = new EndpointConfiguration("Samples.ManualUnsubscribe.Publisher");
        endpointConfiguration.UseTransport<MsmqTransport>();

        var persistence = endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();
        persistence.SubscriptionQueue("Samples.ManualUnsubscribe.Publisher.Subscriptions");

        endpointConfiguration.DisableFeature<TimeoutManager>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press 'enter' to publish the SomethingHappened event");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key == ConsoleKey.Enter)
            {
                var somethingHappened = new SomethingHappened();
                await endpointInstance.Publish(somethingHappened)
                    .ConfigureAwait(false);
                Console.WriteLine("Published SomethingHappened Event.");
            }
            else
            {
                break;
            }
        }

        await endpointInstance.Stop()
                .ConfigureAwait(false);
    }
}