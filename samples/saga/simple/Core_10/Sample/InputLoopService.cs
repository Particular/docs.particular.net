using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sample
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine();
            Console.WriteLine("Storage locations:");
            Console.WriteLine($"Learning Persister: {LearningLocationHelper.SagaDirectory}");
            Console.WriteLine($"Learning Transport: {LearningLocationHelper.TransportDirectory}");

            Console.WriteLine();
            Console.WriteLine("Press 'Enter' to send a StartOrder message");
            Console.WriteLine();

            while (true)
            {
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    break;
                }
                var orderId = Guid.NewGuid();
                var startOrder = new StartOrder
                {
                    OrderId = orderId
                };
                await messageSession.SendLocal(startOrder, cancellationToken: stoppingToken);
                Console.WriteLine($"Sent StartOrder with OrderId {orderId}.");
            }

        }
    }

}
