using Azure.Storage.Blobs;
using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.AzureBlobStorageDataBus.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.AzureBlobStorageDataBus.Sender");

        #region ConfiguringDataBusLocation

        var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");
        var dataBus = endpointConfiguration.UseDataBus<AzureDataBus, SystemJsonDataBusSerializer>()
            .Container("testcontainer")
            .UseBlobServiceClient(blobServiceClient);

        #endregion

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
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

        #region SendMessageLargePayload

        var message = new MessageWithLargePayload
        {
            Description = "This message contains a large payload that will be sent on the Azure data bus",
            LargePayload = new DataBusProperty<byte[]>(new byte[1024 * 1024 * 5]) // 5MB
        };
        await messageSession.Send("Samples.AzureBlobStorageDataBus.Receiver", message);

        #endregion

        Console.WriteLine("Message sent.");
    }
}
