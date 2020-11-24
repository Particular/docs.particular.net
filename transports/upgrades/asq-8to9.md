---
title: Azure Storage Queues Transport Upgrade Version 8 to 9
summary: Migration instructions on how to upgrade Azure Storage Queues Transport from Version 8 to 9.
reviewed: 2020-11-13
component: ASQ
related:
- transports/azure-storage-queues
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Package name

This package was formerly known as `NServiceBus.Azure.Transports.WindowsAzureStorageQueues`.
As of version 9 the package name is `NServiceBus.Transport.AzureStorageQueues`.

## Types from namespace `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` moved to `NServiceBus.Transport.AzureStorageQueues`

Certain advanced configuration APIs have been moved from the namespace `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` to `NServiceBus.Transport.AzureStorageQueues`Code must to be adjusted accordingly. A straight forward way is to search and replace

```
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;
```

with

```
using NServiceBus.Transport.AzureStorageQueues;
```

## Move to .NET 4.7.2

The minimum .NET Framework version is [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472).

**All projects must be updated to .NET Framework 4.7.2 before upgrading to NServiceBus.Transport.AzureStorageQueues version 9.**

It is recommended to update to .NET Framework 4.7.2 and perform a full migration to production **before** updating to version 9. This will isolate any issues that may occur.

For solutions with many projects, the [Target Framework Migrator](https://marketplace.visualstudio.com/items?itemName=PavelSamokha.TargetFrameworkMigrator) Visual Studio extension can reduce the manual effort required in performing an upgrade.

## Timeout manager

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout manager backwards compatibility mode obsolete. If backwards compatibility mode was enabled these APIs must be removed.

## Account aliases

Account aliases are enforced by default and `transport.UseAccountAliasesInsteadOfConnectionStrings()` is deprecated. See [Configuration API](/transports/azure-storage-queues/configuration.md#connection-strings-using-aliases-for-connection-strings-to-storage-accounts).

## Using clients

Queue, Blob and Table clients are the recommended way to configure the transport instead of using connection strings. Connections strings are still supported but will be removed in the future versions. See [Configuration API](/transports/azure-storage-queues/configuration.md#configuration-api).

## Transport connection string

The connection string set via `transport.ConnectionString()` is no longer mandatory and is only needed when clients for queue, blob or table are not configured.