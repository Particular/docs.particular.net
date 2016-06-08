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

        var endpointConfiguration = new EndpointConfiguration("Samples.Throttling");
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

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("\r\nPress any key to stop program\r\n");

            Console.WriteLine("Sending messages...");
            for (var i = 0; i < 100; i++)
            {
                var searchGitHub = new SearchGitHub
                {
                    Repository = "NServiceBus",
                    RepositoryOwner = "Particular",
                    SearchFor = "IBus"
                };
                await endpointInstance.SendLocal(searchGitHub)
                    .ConfigureAwait(false);
            }
            Console.WriteLine("Messages sent.");
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}