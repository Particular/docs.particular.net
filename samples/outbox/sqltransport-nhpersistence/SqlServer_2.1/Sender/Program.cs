using System;
using System.Linq;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SQLNHibernateOutbox.Sender";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var busConfiguration = new BusConfiguration();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EndpointName("Samples.SQLNHibernateOutbox.Sender");

        #region SenderConfiguration

        busConfiguration.UseTransport<SqlServerTransport>();
        busConfiguration.UsePersistence<NHibernatePersistence>();
        busConfiguration.EnableOutbox();

        #endregion

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to publish a message");
            Console.WriteLine("Press any key to exit");
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                var orderSubmitted = new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                };
                bus.Publish(orderSubmitted);
            }
        }
    }
}