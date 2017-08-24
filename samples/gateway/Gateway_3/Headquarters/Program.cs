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
        var endpointConfiguration = new EndpointConfiguration("Samples.Gateway.Headquarters");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableFeature<Gateway>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();


        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press 'Enter' to send a message to RemoteSite which will reply.");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            string[] siteKeys =
            {
                "RemoteSite"
            };
            var priceUpdated = new PriceUpdated
            {
                ProductId = 2,
                NewPrice = 100.0,
                ValidFrom = DateTime.Today,
            };
            await endpointInstance.SendToSites(siteKeys, priceUpdated)
                .ConfigureAwait(false);

            Console.WriteLine("Message sent, check the output in RemoteSite");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}