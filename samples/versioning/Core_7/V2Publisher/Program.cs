using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Versioning.V2Publisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.Versioning.V2Publisher");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to publish a message");
        Console.WriteLine("Press any key to exit");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            await endpointInstance.Publish<V2.Messages.ISomethingHappened>(sh =>
            {
                sh.SomeData = 1;
                sh.MoreInfo = "It's a secret.";
            })
            .ConfigureAwait(false);

            Console.WriteLine("Published event.");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}