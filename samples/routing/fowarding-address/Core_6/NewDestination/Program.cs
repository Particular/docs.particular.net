using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            Console.Title = "NewDestination";
            var config = new EndpointConfiguration("NewDestination");
            config.UseTransport<LearningTransport>();

            var endpoint = await Endpoint.Start(config).ConfigureAwait(false);

            Console.WriteLine("Endpoint Started. [ENTER] to exit");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
            }

            await endpoint.Stop().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}