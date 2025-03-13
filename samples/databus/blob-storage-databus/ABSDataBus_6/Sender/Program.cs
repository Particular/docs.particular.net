using Azure.Storage.Blobs;
using NServiceBus;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sender;


Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

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

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
