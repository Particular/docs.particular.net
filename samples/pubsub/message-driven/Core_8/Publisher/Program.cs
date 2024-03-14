using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PubSub.MessageDrivenPublisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.MessageDrivenPublisher");
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new MsmqTransport());

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await PublishOrderEvent(endpointInstance);
        await endpointInstance.Stop();
    }

    static async Task PublishOrderEvent(IEndpointInstance endpointInstance)
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
                var orderReceived = new OrderReceived
                {
                    OrderId = orderReceivedId
                };
                await endpointInstance.Publish(orderReceived);
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
