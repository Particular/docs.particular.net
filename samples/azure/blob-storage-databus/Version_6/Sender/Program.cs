using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.DataBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.AzureBlobStorageDataBus.Sender");
        endpointConfiguration.UseSerialization<JsonSerializer>();

        #region ConfiguringDataBusLocation

        endpointConfiguration.UseDataBus<AzureDataBus>()
            .ConnectionString("UseDevelopmentStorage=true");

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await Run(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    static async Task Run(IMessageSession messageSession)
    {
        Console.WriteLine("Press 'Enter' to send a large message (>4MB)");
        Console.WriteLine("To exit, press Ctrl + C");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.Enter)
            {
                await SendMessageLargePayload(messageSession);
            }
        }
    }

    static async Task SendMessageLargePayload(IMessageSession messageSession)
    {
        Console.WriteLine("Sending message...");

        #region SendMessageLargePayload

        MessageWithLargePayload message = new MessageWithLargePayload
        {
            Description = "This message contains a large payload that will be sent on the Azure data bus",
            LargePayload = new DataBusProperty<byte[]>(new byte[1024*1024*5]) // 5MB
        };
        await messageSession.Send("Samples.AzureBlobStorageDataBus.Receiver", message);

        #endregion

        Console.WriteLine("Message sent.");
    }
}