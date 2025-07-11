using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class DeleteBlobFunction(BlobContainerClient containerClient, ILogger<DeleteBlobFunction> logger)
{
    #region DeleteBlobFunction

    [Function("DeleteBlob")]
    public async Task DeleteBlob([ActivityTrigger] DataBusBlobData blobData)
    {
        var blob = containerClient.GetBlobClient(blobData.Name);

        logger.LogInformation("Deleting blob at {Name}", blobData.Name);

        await blob.DeleteIfExistsAsync();
    }

    #endregion
}