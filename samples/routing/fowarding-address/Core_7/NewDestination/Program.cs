using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "NewDestination";
        var config = new EndpointConfiguration("NewDestination");
        config.UseTransport<LearningTransport>();

        var endpoint = await Endpoint.Start(config)
            .ConfigureAwait(false);

        Console.WriteLine("Endpoint Started. Press any key to exit");

        Console.ReadKey();

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}