using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.NHibernate.Client";
        var busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.NHibernate.Client");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<NHibernatePersistence>();

        var bus = Bus.Create(busConfiguration).Start();

        Console.WriteLine("Press 'enter' to send a StartOrder messages");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var orderId = Guid.NewGuid();
            var startOrder = new StartOrder
            {
                OrderId = orderId
            };

            bus.Send(startOrder);
            Console.WriteLine($"StartOrder Message sent with OrderId {orderId}");
        }

        bus.Dispose();
    }
}