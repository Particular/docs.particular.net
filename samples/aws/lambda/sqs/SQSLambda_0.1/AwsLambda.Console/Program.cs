using NServiceBus;
using System.Threading.Tasks;

namespace AwsLambda.Console
{
    using System;

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
                        await (asbEndpoint?.Stop() ?? Task.CompletedTask).ConfigureAwait(false);
                        return;
                }
            }
        }

        private static IEndpointInstance asbEndpoint;

        static async Task SendMessage()
        {
            if (asbEndpoint == null)
            {
                var endpointConfiguration = new EndpointConfiguration("AwsLambda.Sender");
                endpointConfiguration.SendOnly();
                endpointConfiguration.UsePersistence<InMemoryPersistence>();
                endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

                var transport = endpointConfiguration.UseTransport<SqsTransport>();
                
                asbEndpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            }

            await asbEndpoint.Send("AwsLambda.SQSTrigger", new TriggerMessage())
                .ConfigureAwait(false);
        }
    }
}
