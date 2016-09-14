using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;

public class StorageHelper
{
    public static CloudTableClient GetTableClient()
    {
        var connectionString = Environment.GetEnvironmentVariable("AzureStoragePersistence.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureStoragePersistence.ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var storageaccount = CloudStorageAccount.Parse(connectionString);
        var client = storageaccount.CreateCloudTableClient();
        client.DefaultRequestOptions.RetryPolicy = new ExponentialRetry();
        return client;
    }
}