using Azure.Storage.Blobs;
using NServiceBus;
using NServiceBus.ClaimCheck;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.AzureBlobStorageDataBus.Receiver");

        var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");

        var claimCheck = endpointConfiguration.UseClaimCheck<AzureDataBus, SystemJsonClaimCheckSerializer>()
            .Container("testcontainer")
            .UseBlobServiceClient(blobServiceClient);


        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
