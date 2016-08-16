using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;

[TestFixture]
public class AzureHelper
{
    [Test]
    [Explicit]
    public Task WriteOutData()
    {
        #region UsingHelpers

        WriteOutTable("OrderSagaData", false);
        WriteOutTable("Subscription", true);
        WriteOutTable("TimeoutDataTableName", false);
        WriteOutTable("TimeoutManagerDataTable", false);
        return WriteOutBlobContainer("timeoutstate");

        #endregion
    }

    #region WriteOutBlobContainer

    static async Task WriteOutBlobContainer(string containerName)
    {
        var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        var tableClient = storageAccount.CreateCloudBlobClient();
        var container = tableClient.GetContainerReference(containerName);
        Debug.WriteLine($"'{containerName}' container contents");
        foreach (var blob in container.ListBlobs())
        {
            var name = blob.Uri.AbsolutePath.Split('/').Last();
            Debug.WriteLine($"  Blob:= {name}");
            var blockBlobReference = container.GetBlockBlobReference(name);
            var text = await blockBlobReference.DownloadTextAsync()
                .ConfigureAwait(false);
            Debug.WriteLine($"    {text}");
        }
        Debug.WriteLine("");
    }

    #endregion

    #region WriteOutTable

    static void WriteOutTable(string tableName, bool decodeRowKey)
    {
        var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        var tableClient = storageAccount.CreateCloudTableClient();
        var table = tableClient.GetTableReference(tableName);
        var entities = table.ExecuteQuery(new TableQuery()).ToList();
        Debug.WriteLine($"'{tableName}' table contents");
        foreach (var entity in entities)
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