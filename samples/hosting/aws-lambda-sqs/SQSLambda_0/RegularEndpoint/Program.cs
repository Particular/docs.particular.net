using System;
using System.Net;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "RegularEndpoint";

        if (endpointInstance == null)
        {
            var endpointConfiguration = new EndpointConfiguration("RegularEndpoint");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
            endpointConfiguration.UseTransport<SqsTransport>();

            endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }
            

        Console.WriteLine("Press [ENTER] to send a message to the SQS trigger queue.");
        Console.WriteLine("Press [Esc] to exit.");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    await SendMessage().ConfigureAwait(false);
                    break;
                case ConsoleKey.Escape:
                    await (endpointInstance?.Stop() ?? Task.CompletedTask).ConfigureAwait(false);
                    return;
            }
        }
    }

    private static IEndpointInstance endpointInstance;
    static readonly ILog Log = LogManager.GetLogger<Program>();

    static async Task SendMessage()
    {
        await endpointInstance.Send("ServerlessEndpoint", new TriggerMessage())
            .ConfigureAwait(false);

        Log.Info("Message sent to the serverless endpoint queue.");
    }
}