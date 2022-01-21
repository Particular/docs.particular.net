using NServiceBus;
using NServiceBus.Logging;
using Shared;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.FullDuplex.Server";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);
        var endpointConfiguration = new EndpointConfiguration("Samples.FullDuplex.Server");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());

        endpointConfiguration.AuditProcessedMessagesTo("audit");

        var containerClient = await AzureAuditBodyStorageConfiguration.GetContainerClient();

        endpointConfiguration.Pipeline.Register(_ => new StoreAuditBodyInAzureBlobStorageBehavior(containerClient), "Writing the body to azure blobstorage");


        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}