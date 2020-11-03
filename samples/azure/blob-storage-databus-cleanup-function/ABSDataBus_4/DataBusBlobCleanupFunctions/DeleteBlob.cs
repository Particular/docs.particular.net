using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public static class DeleteBlobActivityFunction
{
    static DeleteBlobActivityFunction()
    {
        var storageConnectionString = Environment.GetEnvironmentVariable("DataBusStorageAccount");
        var cloudStorageAccount = Microsoft.Azure.Storage.CloudStorageAccount.Parse(storageConnectionString);
        cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
    }

    [FunctionName("DeleteBlob")]
    // public static async Task DeleteBlob([ActivityTrigger] IDurableActivityContext context, ILogger log)
    public static async Task DeleteBlob([ActivityTrigger] DataBusBlobData blobData, ILogger log)
    {
        // var blobData = context.GetInput<DataBusBlobData>();
        var blob = await cloudBlobClient.GetBlobReferenceFromServerAsync(new Uri(blobData.Path));
        log.LogInformation($"Deleting blob at {blobData.Path}");
        await blob.DeleteIfExistsAsync();
    }

    static CloudBlobClient cloudBlobClient;
}