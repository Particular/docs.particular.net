using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Endpoint1";
        var endpointConfiguration = new EndpointConfiguration("Endpoint1");

        #region UseTransport

        endpointConfiguration.UseTransport<LearningTransport>();

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.DisableLegacyRetriesSatellite();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press S to send a message");
        Console.WriteLine("Press D to send a delayed message");
        Console.WriteLine("Press any key to exit");

        #region StartMessageInteraction

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key == ConsoleKey.D)
            {
                var message = new TheMessage();
                var sendOptions = new SendOptions();
                sendOptions.DelayDeliveryWith(TimeSpan.FromSeconds(10));
                sendOptions.SetDestination("Endpoint2");
                await endpointInstance.Send(message, sendOptions)
                    .ConfigureAwait(false);

                Console.WriteLine("Sent a delayed message");
                continue;
            }
            if (key.Key == ConsoleKey.S)
            {
                var message = new TheMessage();
                var sendOptions = new SendOptions();
                sendOptions.SetDestination("Endpoint2");
                await endpointInstance.Send(message, sendOptions)
                    .ConfigureAwait(false);

                Console.WriteLine("Sent a message");
                continue;
            }
            break;
        }

        #endregion

        Console.WriteLine("Message sent. Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}