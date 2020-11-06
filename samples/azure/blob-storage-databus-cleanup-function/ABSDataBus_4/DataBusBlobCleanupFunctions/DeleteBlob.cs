using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public class DeleteBlobFunction
{
    readonly CloudBlobClient cloudBlobClient;

    public DeleteBlobFunction(CloudBlobClient cloudBlobClient)
    {
        this.cloudBlobClient = cloudBlobClient;
    }

    #region DeleteBlobFunction

    [FunctionName("DeleteBlob")]
    public async Task DeleteBlob([ActivityTrigger] DataBusBlobData blobData, ILogger log)
    {
        var blob = await cloudBlobClient.GetBlobReferenceFromServerAsync(new Uri(blobData.Path));
        log.LogInformation($"Deleting blob at {blobData.Path}");
        await blob.DeleteIfExistsAsync();
    }

    #endregion
}