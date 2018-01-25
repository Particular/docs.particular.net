using System;
using System.Threading.Tasks;
using NServiceBus;
using Publisher.Contracts;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ConsumerDrivenContracts.Producer";
        var endpointConfiguration = new EndpointConfiguration("Samples.ConsumerDrivenContracts.Producer");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Start(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'p' to publish event");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var myEvent = new MyEvent
            {
                Consumer1Property = "Consumer1Info",
                Consumer2Property = "Consumer2Info"
            };
            await endpointInstance.Publish(myEvent)
                .ConfigureAwait(false);
        }
    }
}