﻿using System;
using Microsoft.Azure.Cosmos.Table;

public class StorageHelper
{
    public static CloudTableClient GetTableClient()
    {
        var connectionString = Environment.GetEnvironmentVariable("AzureStoragePersistence_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureStoragePersistence_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var storageaccount = CloudStorageAccount.Parse(connectionString);
        var client = storageaccount.CreateCloudTableClient();
        client.DefaultRequestOptions.RetryPolicy = new ExponentialRetry();
        return client;
    }
}