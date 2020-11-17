using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Cosmos.Table;
using NUnit.Framework;

[TestFixture]
public class AzureHelper
{
    [Test]
    [Explicit]
    public async Task WriteOutData()
    {
        #region UsingHelpers

        await WriteOutTable("OrderSagaData", false).ConfigureAwait(false);
        await WriteOutTable("Subscription", true).ConfigureAwait(false);

        #endregion
    }

    #region WriteOutBlobContainer

    static async Task WriteOutBlobContainer(string containerName)
    {
        var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        Debug.WriteLine($"'{containerName}' container contents");
        var pages = container.GetBlobsAsync().AsPages();
        await foreach (Azure.Page<BlobItem> blobPage in pages)
        {
            foreach (var blobItem in blobPage.Values)
            {
                var name = blobItem.Name;
                Debug.WriteLine($"  Blob:= {name}");
                var blobClient = container.GetBlobClient(name);
                using (var memoryStream = new MemoryStream())
                {
                    await blobClient.DownloadToAsync(memoryStream)
                        .ConfigureAwait(false);
                    var text = Encoding.UTF8.GetString(memoryStream.ToArray());
                    Debug.WriteLine($"    {text}");
                }
            }
        }
        Debug.WriteLine("");
    }

    #endregion

    #region WriteOutTable

    static async Task WriteOutTable(string tableName, bool decodeRowKey)
    {
        var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        var tableClient = storageAccount.CreateCloudTableClient();
        var table = tableClient.GetTableReference(tableName);
        Debug.WriteLine($"'{tableName}' table contents");
        TableContinuationToken token = null;
        do
        {
            var result = await table.ExecuteQuerySegmentedAsync(new TableQuery(), token).ConfigureAwait(false);
            token = result.ContinuationToken;

            foreach (var entity in result.Results)
            {
                Debug.WriteLine($"  PartitionKey:= {entity.PartitionKey}");
                Debug.WriteLine($"    RowKey:= {entity.RowKey}");
                if (decodeRowKey)
                {
                    var decodedRowKey = entity.RowKey.DecodeFromKey();
                    Debug.WriteLine($"    DecodedRowKey:= {decodedRowKey}");
                }
                foreach (var property in entity.Properties)
                {
                    var propertyAsObject = property.Value.PropertyAsObject
                        .ToString().Truncate(50);
                    Debug.WriteLine($"    {property.Key}:= {propertyAsObject}");
                }
            }
        }
        while (token != null);

        Debug.WriteLine("");
    }

    #endregion

    [Test]
    [Explicit]
    public async Task DeleteData()
    {
        await DeleteTable("OrderSagaData")
            .ConfigureAwait(false);
        await DeleteTable("Subscription")
            .ConfigureAwait(false);
    }

    static Task DeleteTable(string tableName)
    {
        var developmentStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        var cloudTableClient = developmentStorageAccount.CreateCloudTableClient();
        var tableReference = cloudTableClient.GetTableReference(tableName);
        return tableReference.DeleteIfExistsAsync();
    }

    static Task DeleteBlobContainer(string containerName)
    {
        var containerClient = new BlobContainerClient("UseDevelopmentStorage=true", containerName);
        return containerClient.DeleteIfExistsAsync();
    }
}