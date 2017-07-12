using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.ZeroFormatter;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Serialization.ZeroFormatter";

        #region config

        var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.ZeroFormatter");
        endpointConfiguration.UseSerialization<ZeroSerializer>();

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        #region messagesend

        var message = new CreateOrder
        {
            OrderId = 9,
            Date = DateTime.Now.Ticks,
            CustomerId = 12,
        };
        await endpointInstance.SendLocal(message)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}