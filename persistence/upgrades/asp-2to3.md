---
title: Azure Storage Persistence Upgrade Version 2 to 3
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

## Package renamed to NServiceBus.Persistence.AzureTable

This package was formerly known as `NServiceBus.Persistence.AzureStorage`.
As of version 3 the package name is `NServiceBus.Persistence.AzureTable`.

### Types from namespace `NServiceBus.Persistence.AzureStorage` moved to `NServiceBus.Persistence.AzureTable`

Certain advanced configuration APIs have been moved from the namespace `NServiceBus.Persistence.AzureStorage` to `using NServiceBus.Persistence.AzureTable`.
The code must be adjusted accordingly. A straight forward way is to perform a search and replace:

```
using NServiceBus.Persistence.AzureStorage;
```

to

```
using NServiceBus.Persistence.AzureTable;
```

## Move to .NET 4.7.2

The minimum .NET Framework version is [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472).

**All projects must be updated to .NET Framework 4.7.2 before upgrading to NServiceBus.Persistence.AzureTable version 3.**

It is recommended to update to .NET Framework 4.7.2 and perform a full migration to production **before** updating to version 3. This will isolate any issues that may occur.

For solutions with many projects, the [Target Framework Migrator](https://marketplace.visualstudio.com/items?itemName=PavelSamokha.TargetFrameworkMigrator) Visual Studio extension can reduce the manual effort required in performing an upgrade.

## Compatibility

The package is fully compatible with the NServiceBus.Persistence.AzureStorage version 1 and 2. It supports both sagas that use secondary index entries as well as sagas that don't have a secondary index entries. By default the persister operates in the compatibility mode but doesn't fall back to full table scans. If compatibility with sagas stored with Version 1 of the persister is required full table scan has to be [enabled](/persistence/azure-table/configuration.md#saga-configuration).

For newly introduced endpoints it is encouraged to disable the compatibility mode due to [performance reasons](/persistence/azure-table/performance-tuning.md)/

## Support for Azure Table Storage and Azure Cosmos DB Table API

The Azure Table Persistence supports both Table Storage and Azure Cosmos DB Table API.

### Migrating from Azure Tables to Azure Cosmos Tables

For more information on how to migrate from Azure storage tables to Cosmos tables, follow [the migration guide](/persistence/azure-table/migration-from-azure-storage-table-to-cosmos-table.md).

## Transactionality

The Azure Table Persistence has been enhanced to leverage transactional API to atomically store data when using sagas or outbox.
Multiple operations are atomically stored by making use of the [TableBatchOperation API](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.table.tablebatchoperation?view=azure-dotnet), only when the data is stored in the same partition within a container

Note that this is not the default. To enable transactionality, a custom behavior needs to be put in place to identify the partition key. The [documentation](/persistence/azure-table/transactions.md) explains the details on how to do this, including a [sample](/samples/azure/azure-table/transactions) as well.

## Installers

To make sure the persistence complies with the principle of least priviledge, the previous `CreateSchema`-method has been deprecated in favour of integration with the Installers-API available on the endpoint configuration.

To enable the persistence to create the needed table(s), the endpoint will need to `EnableInstallers()`, which will result in the tables being created at endpoint startup when a default table was set, or at runtime when the table information is made available.

To opt out of creating tables while still making use of the capabilities provided by `EnableInstallers()`, the `DisableTableCreation` method can be invoked.

## Timeout storage

The [timeout manager has been removed in `NServiceBus.Core` version 8](/nservicebus/upgrades/7to8/#timeout-manager-removed). Therefore, the timeout manager was removed in this version of the persister.

- Any timeout-related configuration APIs can be safely removed.
- Database tables must be manually removed from storage.

NOTE: There is no automatic migration of timeout data. See [Timeout manager removed - Data migration](/nservicebus/upgrades/7to8/#timeout-manager-removed-data-migration).
