using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "OriginalDestination";

        #region forward-messages-after-processing

        var config = new EndpointConfiguration("OriginalDestination");
        config.UseTransport<LearningTransport>();

        config.ForwardMessagesAfterProcessingTo("UpgradedDestination");

        #endregion

        var endpoint = await Endpoint.Start(config)
            .ConfigureAwait(false);

        Console.WriteLine("Endpoint Started. Press any key to exit");

        Console.ReadKey();

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}