using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Sender";
        var endpointConfiguration = new EndpointConfiguration("FixMalformedMessages.Sender");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        var routing = endpointConfiguration.UseTransport(new LearningTransport());
        routing.RouteToEndpoint(typeof(SimpleMessage), "FixMalformedMessages.Receiver");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
        while (true)
        {
            var key = Console.ReadKey();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var guid = Guid.NewGuid();

            var simpleMessage = new SimpleMessage
            {
                Id = guid.ToString().ToLowerInvariant()
            };
            await endpointInstance.Send(simpleMessage)
                .ConfigureAwait(false);

            Console.WriteLine($"Sent a new message with Id = {guid}.");

            Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}