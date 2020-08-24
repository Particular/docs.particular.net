using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "AwsLambda.Sender";

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
                    await (sqsEndpoint?.Stop() ?? Task.CompletedTask).ConfigureAwait(false);
                    return;
            }
        }
    }

    private static IEndpointInstance sqsEndpoint;

    static async Task SendMessage()
    {
        if (sqsEndpoint == null)
        {
            var endpointConfiguration = new EndpointConfiguration("AwsLambda.Sender");
            endpointConfiguration.SendFailedMessagesTo("ErrorAwsLambdaSQSTrigger");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

            var transport = endpointConfiguration.UseTransport<SqsTransport>();
                
            sqsEndpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }

        await sqsEndpoint.Send("AwsLambdaSQSTrigger", new TriggerMessage())
            .ConfigureAwait(false);
    }
}