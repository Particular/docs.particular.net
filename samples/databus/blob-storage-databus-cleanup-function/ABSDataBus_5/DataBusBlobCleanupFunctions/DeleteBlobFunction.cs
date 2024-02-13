using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class DeleteBlobFunction
{
    public DeleteBlobFunction(BlobContainerClient containerClient, ILogger<DeleteBlobFunction> logger)
    {
        this.containerClient = containerClient;
        this.logger = logger;
    }

    #region DeleteBlobFunction

    [Function("DeleteBlob")]
    public async Task DeleteBlob([ActivityTrigger] DataBusBlobData blobData)
    {
        var blob = containerClient.GetBlobClient(blobData.Name);

        logger.LogInformation("Deleting blob at {name}", blobData.Name);

        await blob.DeleteIfExistsAsync();
    }

    #endregion

    private readonly BlobContainerClient containerClient;
    private readonly ILogger<DeleteBlobFunction> logger;
}
