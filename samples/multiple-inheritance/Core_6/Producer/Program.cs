using System;
using System.Threading.Tasks;
using NServiceBus;
using Publisher.Contracts;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.MultipleInheritance.Producer";
        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleInheritance.Producer");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            await Start(endpointInstance)
                .ConfigureAwait(false);
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }

    static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'p' to publish event");
        Console.WriteLine("Press any other key to exit");

        #region PublishLoop

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

        #endregion
    }
}