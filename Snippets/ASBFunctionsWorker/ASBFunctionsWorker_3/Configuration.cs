using Azure.Storage.Blobs;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;
using System.Threading.Tasks;

#region asb-function-isolated-configuration
[assembly: NServiceBusTriggerFunction("WorkerDemoEndpoint")]

public class Program
{
    public static Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus()
            .Build();

        return host.RunAsync();
    }
}
#endregion asb-function-isolated-configuration

class EnableDiagnostics
{
    #region asb-function-isolated-enable-diagnostics
    public static Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus(configuration =>
            {
                configuration.LogDiagnostics();
            })
            .Build();

        return host.RunAsync();
    }
    #endregion
}

class EnableDiagnosticsBlob
{
    public static Task Main()
    {
        var endpointName = "ASBWorkerEndpoint";

        #region asb-function-iso-diagnostics-blob
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus(configuration =>
            {
                configuration.AdvancedConfiguration.CustomDiagnosticsWriter(async diagnostics =>
                {
                    var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                    var blobServiceClient = new BlobServiceClient(connectionString);

                    var containerClient = blobServiceClient.GetBlobContainerClient("diagnostics");
                    await containerClient.CreateIfNotExistsAsync();

                    var blobName = $"{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss-fff}-{endpointName}-configuration.txt";
                    var blobClient = containerClient.GetBlobClient(blobName);
                    await blobClient.UploadAsync(BinaryData.FromString(diagnostics));
                });
            })
            .Build();
        #endregion
        return host.RunAsync();
    }
}

class ConfigureErrorQueue
{
    #region asb-function-isolated-configure-error-queue
    public static Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus(configuration =>
            {
                // Change the error queue name:
                configuration.AdvancedConfiguration.SendFailedMessagesTo("my-custom-error-queue");

                // Or disable the error queue to let ASB native dead-lettering handle repeated failures:
                configuration.DoNotSendMessagesToErrorQueue();
            })
            .Build();

        return host.RunAsync();
    }
    #endregion
}