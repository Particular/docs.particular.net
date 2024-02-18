using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        var storageConnectionString = Environment.GetEnvironmentVariable("DataBusStorageAccount");
        var blobServiceClient = new BlobServiceClient(storageConnectionString);

        services.AddSingleton((services) => blobServiceClient.GetBlobContainerClient("databus"));
    })
    .Build();

host.Run();
