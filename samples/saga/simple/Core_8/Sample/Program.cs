using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        _ = Task.Run(() => Particular.PlatformLauncher.Launch());

        Console.Title = "Samples.SimpleSaga";
        var endpointConfiguration = new EndpointConfiguration("Samples.SimpleSaga");
        endpointConfiguration.AuditSagaStateChanges(serviceControlQueue: "audit");
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        #region config

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        #endregion

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
            var orderId = Guid.Parse("51E8A5C6-E8BF-40FB-AD58-B14E5A14D843");
            var startOrder = new StartOrder
            {
                OrderId = orderId
            };
            await endpointInstance.SendLocal(startOrder)
                .ConfigureAwait(false);
            Console.WriteLine($"Sent StartOrder with OrderId {orderId}.");
            await Task.Delay(100);
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
