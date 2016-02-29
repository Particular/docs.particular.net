using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Throttling";
        #region Configuration
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.Throttling");
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        #endregion
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region RegisterBehavior
        endpointConfiguration.Pipeline.Register(
                   "GitHub API Throttling",
                   typeof(ThrottlingBehavior),
                   "implements API throttling for GitHub APIs");
        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("\r\nPress any key to stop program\r\n");

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
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}