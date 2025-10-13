using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Sender";

var endpointConfiguration = new EndpointConfiguration("Samples.AzureBlobStorageDataBus.Sender");

#region ConfiguringDataBusLocation

var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");

var claimCheck = endpointConfiguration.UseClaimCheck<AzureClaimCheck, SystemJsonClaimCheckSerializer>()
    .Container("testcontainer")
    .UseBlobServiceClient(blobServiceClient);

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

Console.WriteLine("Starting...");

var builder = Host.CreateApplicationBuilder(args);

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.StartAsync();

var messageSession = app.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'Enter' to send a large message (>4MB)");

while (true)
{
    var key = Console.ReadKey();

    if (key.Key == ConsoleKey.Enter)
    {
        await SendMessageLargePayload(messageSession);
    }
    else
    {
        break;
    }
}

await app.StopAsync();
return;

static async Task SendMessageLargePayload(IMessageSession messageSession)
{
    Console.WriteLine("Sending message...");

    #region SendMessageLargePayload

    var message = new MessageWithLargePayload
    {
        Description = "This message contains a large payload that will be sent on the Azure data bus",
        LargePayload = new ClaimCheckProperty<byte[]>(new byte[1024 * 1024 * 5]) // 5MB
    };
    await messageSession.Send("Samples.AzureBlobStorageDataBus.Receiver", message);

    #endregion

    Console.WriteLine("Message sent.");
}