using System;
using System.Linq;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.StoreAndForwardSender";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        Random random = new Random();
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.StoreAndForwardSender");

        #region SenderConfiguration

        busConfiguration.UseTransport<SqlServerTransport>();
        busConfiguration.UsePersistence<NHibernatePersistence>();
        busConfiguration.Pipeline.Register<ForwardBehavior.Registration>();
        busConfiguration.Pipeline.Register<SendThroughLocalQueueBehavior.Registration>();

        #endregion

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to publish a message");
            Console.WriteLine("Press any key to exit");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                string orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                bus.Defer(TimeSpan.FromSeconds(1),  new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                });
                Console.WriteLine("Order {0} placed", orderId);
            }
        }
    }
}