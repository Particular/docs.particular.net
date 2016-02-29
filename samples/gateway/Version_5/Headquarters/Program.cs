using System;
using NServiceBus;
using NServiceBus.Features;
using Shared;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Gateway.Headquarters";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Gateway.Headquarters");
        busConfiguration.EnableInstallers();
        busConfiguration.EnableFeature<Gateway>();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'Enter' to send a message to RemoteSite which will reply.");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                string[] siteKeys =
                {
                    "RemoteSite"
                };
                PriceUpdated priceUpdated = new PriceUpdated
                {
                    ProductId = 2,
                    NewPrice = 100.0,
                    ValidFrom = DateTime.Today,
                };
                bus.SendToSites(siteKeys, priceUpdated);

                Console.WriteLine("Message sent, check the output in RemoteSite");
            }
        }
    }
}
