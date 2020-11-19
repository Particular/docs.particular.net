using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.AzureBlobStorageDataBus.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.AzureBlobStorageDataBus.Sender");

        #region ConfiguringDataBusLocation

        var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");
        var dataBus = endpointConfiguration.UseDataBus<AzureDataBus>()
            .Container("testcontainer")
            .UseBlobServiceClient(blobServiceClient);
        
        #endregion

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

        #region SendMessageLargePayload

        var message = new MessageWithLargePayload
        {
            Description = "This message contains a large payload that will be sent on the Azure data bus",
            LargePayload = new DataBusProperty<byte[]>(new byte[1024*1024*5]) // 5MB
        };
        await messageSession.Send("Samples.AzureBlobStorageDataBus.Receiver", message)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("Message sent.");
    }
}