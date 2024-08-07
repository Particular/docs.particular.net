using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "SenderAndReceiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.AzureDataBusCleanupWithFunctions.SenderAndReceiver");

#pragma warning disable CS0618 // Type or member is obsolete
        var dataBus = endpointConfiguration.UseDataBus<AzureDataBus, SystemJsonDataBusSerializer>();
#pragma warning restore CS0618 // Type or member is obsolete
        dataBus.ConnectionString("UseDevelopmentStorage=true");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        await Run(endpointInstance);

        await endpointInstance.Stop();
    }

    static async Task Run(IMessageSession messageSession)
    {
        Console.WriteLine("Press 'Enter' to send a large message (>4MB)");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.Enter)
            {
                await SendMessageLargePayload(messageSession);
            }
            else
            {
                return;
            }
        }
    }

    static async Task SendMessageLargePayload(IMessageSession messageSession)
    {
        Console.WriteLine("Sending message...");

#pragma warning disable CS0618 // Type or member is obsolete
        var message = new MessageWithLargePayload
        {
            Description = "This message contains a large payload that will be sent on the Azure data bus",
            LargePayload = new DataBusProperty<byte[]>(new byte[1024 * 1024 * 5]) // 5MB
        };
#pragma warning restore CS0618 // Type or member is obsolete

        await messageSession.SendLocal(message);

        Console.WriteLine("Message sent.");
    }
}