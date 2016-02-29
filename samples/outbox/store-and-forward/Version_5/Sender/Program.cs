using System;
using System.Linq;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SQLOutboxStoreAndForward.Sender";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        Random random = new Random();
        BusConfiguration busConfiguration = new BusConfiguration();

        #region SenderConfiguration

        busConfiguration.UseTransport<SqlServerTransport>();
        busConfiguration.UsePersistence<NHibernatePersistence>();
        busConfiguration.Pipeline.Register<OutboxLoopbackReceiveBehavior.Registration>();
        busConfiguration.Pipeline.Register<OutboxLoopbackSendBehavior.Registration>();
        busConfiguration.EnableOutbox();

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
                bus.Publish(new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                });
            }
        }
    }
}