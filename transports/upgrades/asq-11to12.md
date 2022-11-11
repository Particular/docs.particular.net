---
title: Azure Storage Queues Transport Upgrade Version 11 to 12
summary: Instructions on how to upgrade the Azure Storage Queues transport from version 11 to 12.
reviewed: 2022-11-11
component: ASQ
related:
- transports/azure-storage-queues
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Switching to `Azure.Data.Tables`

For an overview refer to the official Azure [migration guide](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/tables/Azure.Data.Tables/MigrationGuide.md).

In brief though, instances of `CloudTableClient` need to be replaced with `TableServiceClient`.


### Instantiating classes with clients

Instead of:

```csharp
var transport = new AzureStorageQueueTransport(
    new QueueServiceClient("connection_string"),
    new BlobServiceClient("connection_string"),
    CloudStorageAccount.Parse("connection_string").CreateCloudTableClient());

var accountInfo = new AccountInfo(
    "accountAlias",
    new QueueServiceClient("account_connection_string"),
    CloudStorageAccount.Parse("account_connection_string").CreateCloudTableClient());
```

Use:

```csharp
var transport = new AzureStorageQueueTransport(
    new QueueServiceClient("connection_string"),
    new BlobServiceClient("connection_string"),
    new TableServiceClient("connection_string"));

var accountInfo = new AccountInfo(
    "accountAlias",
    new QueueServiceClient("account_connection_string"),
    new TableServiceClient("account_connection_string"));
```

### Configuring clients
Configuring clients for queue, blob or table is done via the constructor overload. Setting those via endpoint configuration methods is no longer possible.

Instead of:

```csharp
var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
transport.UseQueueServiceClient(new QueueServiceClient("connection_string"));
transport.UseBlobServiceClient(new BlobServiceClient("connection_string"));
transport.UseCloudTableClient(CloudStorageAccount.Parse("connection_string").CreateCloudTableClient());

// or

transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>(
    new QueueServiceClient("connection_string"),
    new BlobServiceClient("connection_string"),
    CloudStorageAccount.Parse("connection_string").CreateCloudTableClient());
);
```

Use:

```csharp
var transport = new AzureStorageQueueTransport(
    new QueueServiceClient("connection_string"),
    new BlobServiceClient("connection_string"),
    new TableServiceClient("connection_string"));
```


### Configuring account routings

Instead of:

```csharp
var accountRouting = transport.AccountRouting();
accountRouting.AddAccount(
    "accountAlias",
    new QueueServiceClient("account_connection_string"),
    CloudStorageAccount.Parse("account_connection_string").CreateCloudTableClient());
```

Use:

```csharp
var accountRouting = transport.AccountRouting();
accountRouting.AddAccount(
    "accountAlias",
    new QueueServiceClient("account_connection_string"),
    new TableServiceClient("account_connection_string"));
```
