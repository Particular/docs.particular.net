using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Sender";

        #region route-message-to-original-destination

        var config = new EndpointConfiguration("Sender");
        var transport = config.UseTransport<LearningTransport>();
        var routing = transport.Routing();

        routing.RouteToEndpoint(typeof(ImportantMessage), "OriginalDestination");

        #endregion

        var endpoint = await Endpoint.Start(config)
            .ConfigureAwait(false);

        Console.WriteLine("Endpoint Started. Press s to send a very important message. Any other key to exit");

        while (Console.ReadKey(true).Key == ConsoleKey.S)
        {
            var importantMessage = new ImportantMessage
            {
                Text = "Hello there"
            };
            await endpoint.Send(importantMessage)
                .ConfigureAwait(false);
        }

        await endpoint.Stop().ConfigureAwait(false);
    }
}
