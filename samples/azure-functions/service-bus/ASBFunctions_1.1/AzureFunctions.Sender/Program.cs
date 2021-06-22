using Microsoft.Extensions.Configuration;
using NServiceBus;
using System.Threading.Tasks;
using System;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "AzureFunctions.Sender";

        Console.WriteLine("Press [ENTER] to send a message to the Azure ServiceBus trigger queue.");
        Console.WriteLine("Press [Esc] to exit.");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    await SendASBMessage();
                    Console.WriteLine("Message sent.");
                    break;
                case ConsoleKey.Escape:
                    await (asbEndpoint?.Stop() ?? Task.CompletedTask);
                    return;
            }
        }
    }

    private static IEndpointInstance asbEndpoint;

    static async Task SendASBMessage()
    {
        if (asbEndpoint == null)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", optional: false)
                .Build();

            var endpointConfiguration = new EndpointConfiguration("FunctionsASBSender");
            endpointConfiguration.SendOnly();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            transport.ConnectionString(config.GetValue<string>("Values:AzureWebJobsServiceBus"));

            asbEndpoint = await Endpoint.Start(endpointConfiguration);
        }

        await asbEndpoint.Send("ASBTriggerQueue", new TriggerMessage());
    }
}