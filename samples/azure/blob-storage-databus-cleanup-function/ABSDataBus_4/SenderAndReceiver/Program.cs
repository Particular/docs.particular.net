using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.AzureDataBusCleanupWithFunctions.SenderAndReceiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.AzureDataBusCleanupWithFunctions.SenderAndReceiver");

        var dataBus = endpointConfiguration.UseDataBus<AzureDataBus>();
        dataBus.ConnectionString("UseDevelopmentStorage=true");

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Run(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
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
                await SendMessageLargePayload(messageSession)
                    .ConfigureAwait(false);
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

        var message = new MessageWithLargePayload
        {
            Description = "This message contains a large payload that will be sent on the Azure data bus",
            LargePayload = new DataBusProperty<byte[]>(new byte[1024*1024*5]) // 5MB
        };
        await messageSession.SendLocal(message)
            .ConfigureAwait(false);

        Console.WriteLine("Message sent.");
    }
}