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



Transactionality
Migration -- PartitionKey
