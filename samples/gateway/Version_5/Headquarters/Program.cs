using System;
using NServiceBus;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Gateway.Headquarters");
        busConfiguration.EnableInstallers();
        busConfiguration.EnableFeature<Gateway>();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'Enter' to send a message to RemoteSite which will reply. To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                string[] siteKeys = {
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
