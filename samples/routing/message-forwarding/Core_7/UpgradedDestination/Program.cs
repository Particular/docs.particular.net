using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "UpgradedDestination";
        var config = new EndpointConfiguration("UpgradedDestination");
        config.UseTransport<LearningTransport>();

        var endpoint = await Endpoint.Start(config)
            .ConfigureAwait(false);

        Console.WriteLine("Endpoint Started. Press any key to exit");

        Console.ReadKey();

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}