using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using Shared;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Gateway.Headquarters";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.Gateway.Headquarters");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableFeature<Gateway>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");


        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
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
                await endpoint.SendToSites(siteKeys, priceUpdated);

                Console.WriteLine("Message sent, check the output in RemoteSite");
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}
