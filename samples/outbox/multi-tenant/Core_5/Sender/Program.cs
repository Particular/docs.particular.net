using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MultiTenant.Sender";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MultiTenant.Sender");

        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesMultiTenantSender;Integrated Security=True";
        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(connection);
        busConfiguration.EnableOutbox();

        SqlHelper.EnsureDatabaseExists(connection);
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press A or B to publish a message (A and B are tenant IDs)");
            var acceptableInput = new List<char> { 'A', 'B' };

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                var uppercaseKey = char.ToUpperInvariant(key.KeyChar);

                if (acceptableInput.Contains(uppercaseKey))
                {
                    var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                    var message = new OrderSubmitted
                    {
                        OrderId = orderId,
                        Value = random.Next(100)
                    };
                    bus.SetMessageHeader(message, "TenantId", uppercaseKey.ToString());
                    bus.Publish(message);
                }
                else
                {
                    Console.WriteLine($"[{uppercaseKey}] is not a valid tenant identifier.");
                }
            }
        }
    }
}