using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "OriginalDestination";

        #region forward-message-to-new-destination

        var endpointConfiguration = new EndpointConfiguration("OriginalDestination");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routing = transport.Routing();

        routing.ForwardToEndpoint(typeof(ImportantMessage), "NewDestination");

        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Endpoint Started. Press any key to exit");
        Console.ReadKey();

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}