---
title: Upgrade from Azure Storage persistence to Version 3
summary: Instructions on how to migrate from Azure Storage Persistence v2 to Azure Table Persistence version 3
reviewed: 2020-11-06
component: ASP
related:
 - persistence/azure-table
redirects:
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Package renamed to NSerivceBus.Persistence.AzureTable

This package was formerly known as `NServiceBus.Persistence.AzureStorage`.
As of version 3 the package name is `NServiceBus.Persistence.AzureTable`.

### Types from namespace `NServiceBus.Persistence.AzureStorage` moved to `NServiceBus.Persistence.AzureTable`

Certain advanced configuration APIs have been moved from the namespace `NServiceBus.Persistence.AzureStorage` to `using NServiceBus.Persistence.AzureTable`.
The code must be adjusted accordingly. A straight forward way is to perform a search and replace:

```
using NServiceBus.Persistence.AzureStorage
```

to

```
using NServiceBus.Persistence.AzureTable;
```

## Move to .NET 4.7.2

The minimum .NET Framework version is [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472).

**All projects must be updated to .NET Framework 4.7.2 before upgrading to NSerivceBus.Persistence.AzureTable version 3.**

It is recommended to update to .NET Framework 4.7.2 and perform a full migration to production **before** updating to version 3. This will isolate any issues that may occur.

For solutions with many projects, the [Target Framework Migrator](https://marketplace.visualstudio.com/items?itemName=PavelSamokha.TargetFrameworkMigrator) Visual Studio extension can reduce the manual effort required in performing an upgrade.

## Support for Table and Cosmos API

The Azure Table Persistence supports both storage in Azure Tables and Azure Cosmos tables.

### Migrating from Azure Tables to Azure Cosmos Tables

For more information on how to migrate from Azure storage tables to Cosmos tables, follow [the migration guide](/persistence/azure-table/migration-from-azure-storage-table-to-cosmos-table.md).

## Transactionality

The Azure Table Persistence has been enhanced to leverage transactions to atomically store data when using sagas or outbox.
Multiple operations are atomically stored by making use of the [TableBatchOperation API](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.table.tablebatchoperation?view=azure-dotnet), only when the data is stored in the same partition within a container

Note that this is not the default. To enable transactionality, a custom behavior needs to be put in place to identify the partition key. The [documentation](/persistence/azure-table/transactions.md) explains the details on how to do this, including some [samples](/samples/azure/azure-table/transactions) as well.

