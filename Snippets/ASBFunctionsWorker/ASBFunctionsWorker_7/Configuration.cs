using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using NServiceBus;

#region asb-function-isolated-configuration
[assembly: NServiceBusTriggerFunction("WorkerDemoEndpoint")]

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.AddNServiceBus();

        var host = builder.Build();

        await host.RunAsync();
    }
}
#endregion asb-function-isolated-configuration

class EnableDiagnostics
{
    #region asb-function-isolated-enable-diagnostics
    public static async Task Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.AddNServiceBus(configuration =>
        {
            configuration.LogDiagnostics();
        });

        var host = builder.Build();

        await host.RunAsync();
    }
    #endregion
}

class EnableDiagnosticsBlob
{
    public static async Task Main(string[] args)
    {
        var endpointName = "ASBWorkerEndpoint";

        #region asb-function-iso-diagnostics-blob
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.AddNServiceBus(configuration =>
        {
            configuration.AdvancedConfiguration.CustomDiagnosticsWriter(
                async (diagnostics, cancellationToken) =>
                {
                    var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                    var blobServiceClient = new BlobServiceClient(connectionString);

                    var containerClient = blobServiceClient.GetBlobContainerClient("diagnostics");
                    await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

                    var blobName = $"{endpointName}-configuration.txt";
                    var blobClient = containerClient.GetBlobClient(blobName);
                    await blobClient.UploadAsync(BinaryData.FromString(diagnostics), cancellationToken);
                });
        });

        var host = builder.Build();
        #endregion
        await host.RunAsync();
    }
}

class ConfigureErrorQueue
{
    #region asb-function-isolated-configure-error-queue
    public static async Task Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.AddNServiceBus(configuration =>
        {
            // Change the error queue name:
            configuration.AdvancedConfiguration.SendFailedMessagesTo("my-custom-error-queue");

            // Or disable the error queue to let ASB native dead-lettering handle repeated failures:
            configuration.DoNotSendMessagesToErrorQueue();
        });

        var host = builder.Build();

        await host.RunAsync();
    }
    #endregion
}