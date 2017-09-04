using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using Microsoft.WindowsAzure.Storage.Blob;

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
        await WriteOutTable("TimeoutDataTableName", false).ConfigureAwait(false);
        await WriteOutTable("TimeoutManagerDataTable", false).ConfigureAwait(false);
        await WriteOutBlobContainer("timeoutstate").ConfigureAwait(false);

        #endregion
    }

    #region WriteOutBlobContainer

    static async Task WriteOutBlobContainer(string containerName)
    {
        var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        var tableClient = storageAccount.CreateCloudBlobClient();
        var container = tableClient.GetContainerReference(containerName);
        Debug.WriteLine($"'{containerName}' container contents");
        BlobContinuationToken token = null;
        do
        {
            var segment = await container.ListBlobsSegmentedAsync(token);
            token = segment.ContinuationToken;
            foreach (var blob in segment.Results)
            {
                var name = blob.Uri.AbsolutePath.Split('/').Last();
                Debug.WriteLine($"  Blob:= {name}");
                var blockBlobReference = container.GetBlockBlobReference(name);
                var text = await blockBlobReference.DownloadTextAsync()
                    .ConfigureAwait(false);
                Debug.WriteLine($"    {text}");
            }
        }
        while (token != null);
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
            var result = await table.ExecuteQuerySegmentedAsync(new TableQuery(), token);
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
        await DeleteBlobContainer("timeoutstate")
            .ConfigureAwait(false);
        await DeleteTable("OrderSagaData")
            .ConfigureAwait(false);
        await DeleteTable("Subscription")
            .ConfigureAwait(false);
        await DeleteTable("TimeoutDataTableName")
            .ConfigureAwait(false);
        await DeleteTable("TimeoutManagerDataTable")
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
        var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        var tableClient = storageAccount.CreateCloudBlobClient();
        var container = tableClient.GetContainerReference(containerName);
        return container.DeleteIfExistsAsync();
    }
}