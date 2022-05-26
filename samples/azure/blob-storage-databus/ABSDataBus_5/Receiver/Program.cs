﻿using Azure.Storage.Blobs;
using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.AzureBlobStorageDataBus.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.AzureBlobStorageDataBus.Receiver");

        var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");
        var dataBus = endpointConfiguration.UseDataBus<AzureDataBus, SystemJsonDataBusSerializer>()
            .Container("testcontainer")
            .UseBlobServiceClient(blobServiceClient);

        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}