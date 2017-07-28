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
        Console.Title = "Samples.Throttling.Sender";

        var endpointConfiguration = new EndpointConfiguration("Samples.Throttling.Sender");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        #region Sending
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Sending messages...");
        for (var i = 0; i < 100; i++)
        {
            var searchGitHub = new SearchGitHub
            {
                Repository = "NServiceBus",
                Owner = "Particular",
                SearchFor = "IBus"
            };
            await endpointInstance.Send("Samples.Throttling.Limited", searchGitHub)
                .ConfigureAwait(false);
        }
        #endregion
        Console.WriteLine("Messages sent.");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}