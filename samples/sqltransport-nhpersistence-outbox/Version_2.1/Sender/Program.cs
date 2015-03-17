using System;
using System.Linq;
using Messages;
using NServiceBus;

namespace Sender
{
    class Program
    {
        static void Main()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
            Random random = new Random();
            BusConfiguration busConfig = new BusConfiguration();

            #region SenderConfiguration

            busConfig.UseTransport<SqlServerTransport>();
            busConfig.UsePersistence<NHibernatePersistence>();
            busConfig.EnableOutbox();

            #endregion

            IBus bus = Bus.Create(busConfig).Start();
            while (true)
            {
                Console.WriteLine("Press <enter> to send a message");
                Console.ReadLine();

                string orderId = new String(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                bus.Publish(new OrderSubmitted
                            {
                                OrderId = orderId,
                                Value = random.Next(100)
                            });
            }
        }
    }
}