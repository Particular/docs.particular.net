using System;
using System.Threading.Tasks;
using NServiceBus;

public class Program
{
    static void Main(string[] args)
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration configuration = new BusConfiguration();
        configuration.EndpointName("Samples.Throttling");
        configuration.UsePersistence<InMemoryPersistence>();
        configuration.SendFailedMessagesTo("error");
        configuration.LimitMessageProcessingConcurrencyTo(1);

        configuration.Pipeline.Register(
                   "GitHub API Throttling",
                   typeof(ThrottlingBehavior),
                   "implements API throttling for GitHub APIs");

        IEndpointInstance endpoint = await Endpoint.Start(configuration);
        try
        {
            Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");

            Console.WriteLine("Sending messages...");
            for (int i = 0; i < 100; i++)
            {
                await endpoint.SendLocal(new SearchGitHub
                {
                    Repository = "NServiceBus",
                    RepositoryOwner = "Particular",
                    SearchFor = "IBus"
                });
            }
            Console.WriteLine("Messages sent.");
            
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}