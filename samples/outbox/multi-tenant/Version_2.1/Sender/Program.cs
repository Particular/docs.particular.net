using System;
using System.Linq;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MultiTenant.Sender";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        Random random = new Random();
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.UseSerialization<JsonSerializer>();

        busConfiguration.UsePersistence<NHibernatePersistence>();
        busConfiguration.EnableOutbox();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press A or B to publish a message (A and B are tenant IDs)");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                char uppercaseKey = char.ToUpperInvariant(key.KeyChar);
                
                string orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                OrderSubmitted message = new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                };
                bus.SetMessageHeader(message, "TenantId", uppercaseKey.ToString());
                bus.Publish(message);
            }
        }
    }
}