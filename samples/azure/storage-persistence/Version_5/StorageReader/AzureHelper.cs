using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;

[TestFixture]
public class AzureHelper
{
    [Test]
    [Explicit]
    public void WriteOutData()
    {
        #region UsingHelpers

        WriteOutTable("OrderSagaData", false);
        WriteOutTable("Subscription", true);
        WriteOutTable("TimeoutDataTableName", false);
        WriteOutTable("TimeoutManagerDataTable", false);
        WriteOutBlobContainer("timeoutstate");

        #endregion
    }

    #region WriteOutBlobContainer

    static void WriteOutBlobContainer(string containerName)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        CloudBlobClient tableClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = tableClient.GetContainerReference(containerName);
        Debug.WriteLine(string.Format("'{0}' container contents", containerName));
        foreach (IListBlobItem blob in container.ListBlobs())
        {
            string name = blob.Uri.AbsolutePath.Split('/').Last();
            Debug.WriteLine("  Blob:= " + name);
            CloudBlockBlob blockBlobReference = container.GetBlockBlobReference(name);
            Debug.WriteLine("    " + blockBlobReference.DownloadText());
        }
        Debug.WriteLine("");
    }

    #endregion

    #region WriteOutTable

    static void WriteOutTable(string tableName, bool decodeRowKey)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
        CloudTable table = tableClient.GetTableReference(tableName);
        List<DynamicTableEntity> entities = table.ExecuteQuery(new TableQuery()).ToList();
        Debug.WriteLine(string.Format("'{0}' table contents", tableName));
        foreach (DynamicTableEntity entity in entities)
        {
            Debug.WriteLine(string.Format("  PartitionKey:= " + entity.PartitionKey));
            Debug.WriteLine(string.Format("    RowKey:= " + entity.RowKey));
            if (decodeRowKey)
            {
                string decodedRowKey = entity.RowKey.DecodeFromKey();
                Debug.WriteLine("    DecodedRowKey:= " + decodedRowKey);
            }
            foreach (KeyValuePair<string, EntityProperty> property in entity.Properties)
            {
                string propertyAsObject = property.Value.PropertyAsObject.ToString().Truncate(50);
                Debug.WriteLine("    {0}:= {1}", property.Key, propertyAsObject);
            }
        }
        Debug.WriteLine("");
    }

    #endregion

    [Test]
    [Explicit]
    public void DeleteData()
    {
        DeleteBlobContainer("timeoutstate");
        DeleteTable("OrderSagaData");
        DeleteTable("Subscription");
        DeleteTable("TimeoutDataTableName");
        DeleteTable("TimeoutManagerDataTable");
    }

    static void DeleteTable(string tableName)
    {
        CloudStorageAccount developmentStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        CloudTableClient cloudTableClient = developmentStorageAccount.CreateCloudTableClient();
        CloudTable tableReference = cloudTableClient.GetTableReference(tableName);
        tableReference.DeleteIfExists();
    }

    static void DeleteBlobContainer(string containerName)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        CloudBlobClient tableClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = tableClient.GetContainerReference(containerName);
        container.DeleteIfExists();
    }
}