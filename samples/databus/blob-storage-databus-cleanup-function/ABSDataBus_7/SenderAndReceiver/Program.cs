using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "SenderAndReceiver";

var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.AzureDataBusCleanupWithFunctions.SenderAndReceiver");

var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");
var claimCheck = endpointConfiguration.UseClaimCheck<AzureClaimCheck, SystemJsonClaimCheckSerializer>()
    .Container("testcontainer")
    .UseBlobServiceClient(blobServiceClient);

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

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

await host.StopAsync();

static async Task SendMessageLargePayload(IMessageSession messageSession)
{
    Console.WriteLine("Sending message...");

    var message = new MessageWithLargePayload
    {
        Description = "This message contains a large payload that will be sent on the Azure data bus",
        LargePayload = new byte[1024 * 1024 * 5] // 5MB
    };

    await messageSession.SendLocal(message);

    Console.WriteLine("Message sent.");
}