using System;
using System.Linq;
using Messages;
using NServiceBus;

class Program
{
    static void Main()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        Random random = new Random();
        BusConfiguration busConfiguration = new BusConfiguration();

        #region SenderConfiguration

        busConfiguration.UseTransport<SqlServerTransport>();
        busConfiguration.UsePersistence<NHibernatePersistence>();
        busConfiguration.EnableOutbox();

        #endregion

        IBus bus = Bus.Create(busConfiguration).Start();
        while (true)
        {
            Console.WriteLine("Press <enter> to send a message");
            Console.ReadLine();

            string orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
            bus.Publish(new OrderSubmitted
            {
                OrderId = orderId,
                Value = random.Next(100)
            });
        }
    }
}