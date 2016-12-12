using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.PubSub.Publisher";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.Publisher");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
     
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
        Console.WriteLine("Press '1' to publish the OrderReceived event");
        Console.WriteLine("Press any other key to exit");

        #region PublishLoop

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderReceivedId = Guid.NewGuid();
            if (key.Key == ConsoleKey.D1)
            {
                    await endpointInstance.Publish<OrderReceived>(m =>
                        {
                            m.OrderId = orderReceivedId;
                        })
                        .ConfigureAwait(false);
                    Console.WriteLine($"Published OrderReceived Event with Id {orderReceivedId}.");
             }
            else
            {
                return;
            }
        }
        #endregion
    }
}