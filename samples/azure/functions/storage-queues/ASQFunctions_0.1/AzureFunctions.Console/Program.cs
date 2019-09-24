using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Features;
using System;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "AzureFunctions.Sender";

        Console.WriteLine("Press [Enter] to send a message to the Azure Storage trigger queue.");
        Console.WriteLine("Press [Esc] to exit.");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    await SendASQMessage();
                    Console.WriteLine("Message sent.");
                    break;
                case ConsoleKey.Escape:
                    await (asqEndpoint?.Stop() ?? Task.CompletedTask);
                    return;
            }
        }
    }

    private static IEndpointInstance asqEndpoint;

    static async Task SendASQMessage()
    {
        if (asqEndpoint == null)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", optional: false)
                .Build();

            var endpointConfiguration = new EndpointConfiguration("FunctionsASQSender");
            endpointConfiguration.SendOnly();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();

            var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
            transport.ConnectionString(config.GetValue<string>("Values:AzureWebJobsStorage"));

            asqEndpoint = await Endpoint.Start(endpointConfiguration);
        }

        await asqEndpoint.Send("ASQTriggerQueue", new TriggerMessage());
    }
}
