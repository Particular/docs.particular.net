using System;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.ClaimCheck;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.AzureBlobStorageDataBus.Receiver");

var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");

var claimCheck = endpointConfiguration.UseClaimCheck<AzureClaimCheck, SystemJsonClaimCheckSerializer>()
    .Container("testcontainer")
    .UseBlobServiceClient(blobServiceClient);

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();
