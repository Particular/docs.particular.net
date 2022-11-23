---
title: Azure Storage Persistence Upgrade Version 4 to 5
summary: Instructions on how to migrate from Azure Table Persistence version 4 to 5
reviewed: 2022-11-06
component: ASP
related:
- persistence/azure-table
redirects:
isUpgradeGuide: true
upgradeGuideCoreVersions:
- 8
---


## Switching to `Azure.Data.Tables`

This version introduces support for [`Azure.Data.Tables`](https://www.nuget.org/packages/Azure.Data.Tables/) and moves away from the deprecated [`Microsoft.Azure.Cosmos.Table`](https://www.nuget.org/packages/Microsoft.Azure.Cosmos.Table/1.0.8) package.

For an overview of the SDK changes, refer to the official Azure [migration guide](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/tables/Azure.Data.Tables/MigrationGuide.md).

In brief though, instances of `CloudTableClient` need to be replaced with `TableServiceClient` and `TableEntity` needs to be replaced with `ITableEntity` when declaring saga entities. Any properties that should be excluded from storage, use the `IgnoreDataMember`-attribute instead of `IgnoreProperty`.

### Passing an instance of the client

Instead of:

```csharp
var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();
var account = CloudStorageAccount.Parse(connection);
var cloudTableClient = account.CreateCloudTableClient();
persistence.UseCloudTableClient(cloudTableClient);
```

Use:

```csharp
var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();
var tableServiceClient = new TableServiceClient(connection);
persistence.UseTableServiceClient(tableServiceClient);
```

### Configuring a client provider

Previously, a custom Cloud table client provider could be created by implementing `IProvideCloudTableClient` which is now replaced with `IProvideTableServiceClient` to reflect the new client type.

See the configuration section for [additional guidance](/persistence/azure-table/configuration.md#configuring-a-table-service-client-provider).

### Compatibility mode

This package continues to be fully compatible with the NServiceBus.Persistence.AzureStorage version 1 and 2. It supports both sagas that use secondary index entries as well as sagas that don't have a secondary index entries.
It's important to note that the compatibility mode does need to be manually enabled. It has been disabled by default in favor of performance.

To opt-in for the compatibility mode, use:

```csharp
var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();
var compatibility = persistence.Compatibility();
compatibility.EnableSecondaryKeyLookupForSagasCorrelatedByProperties();
```