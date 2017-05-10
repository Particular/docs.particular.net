using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Persistence;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SimpleSaga";
        var endpointConfiguration = new EndpointConfiguration("Samples.SimpleSaga");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.DisableLegacyRetriesSatellite();

        #region config

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
        endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #endregion

        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine();
        Console.WriteLine("Storage locations:");
        Console.WriteLine($"Learning Persister: {LearningLocationHelper.SagaDirectory}");
        Console.WriteLine($"Learning Transport: {LearningLocationHelper.TransportDirectory}");

        Console.WriteLine();
        Console.WriteLine("Press 'Enter' to send a StartOrder message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            Console.WriteLine();
            if (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                break;
            }
            var orderId = Guid.NewGuid();
            var startOrder = new StartOrder
            {
                OrderId = orderId
            };
            await endpointInstance.SendLocal(startOrder)
                .ConfigureAwait(false);
            Console.WriteLine($"Sent StartOrder with OrderId {orderId}.");
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

}