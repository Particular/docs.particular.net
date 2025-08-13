using System;
using System.Threading.Tasks;
using NServiceBus;
using Publisher.Contracts;

class Program
{
    static async Task Main()
    {
        Console.Title = "Producer";
        var endpointConfiguration = new EndpointConfiguration("Samples.ConsumerDrivenContracts.Producer");
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await Start(endpointInstance);
        await endpointInstance.Stop();
    }

    static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'p' to publish event");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.P:
                    var myEvent = new MyEvent
                    {
                        Consumer1Property = "Consumer1Info",
                        Consumer2Property = "Consumer2Info"
                    };
                    await endpointInstance.Publish(myEvent);

                    continue;
            }

            return;
        }
    }
}