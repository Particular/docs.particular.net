---
title: Azure Storage Queues Transport Upgrade Version 8 to 9
summary: Migration instructions on how to upgrade Azure Storage Queues Transport from Version 8 to 9.
reviewed: 2020-11-12
component: ASQ
related:
- transports/azure-storage-queues
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Package name

This package was formerly known as `NServiceBus.Azure.Transports.WindowsAzureStorageQueues`.
As of version 9 the package name is `NServiceBus.Transport.AzureStorageQueues`.

### Moved types from namespace `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` to `NServiceBus.Transport.AzureStorageQueues`

Certain advanced configuration APIs have been moved from the namespace `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` to `NServiceBus.Transport.AzureStorageQueues`Code must to be adjusted accordingly. A straight forward way is to search and replace

```
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;
```

with

```
using NServiceBus.Transport.AzureStorageQueues;
```

# .NET Framework

To run the package with .NET Framework will require at least .NET Framework 4.7.2.

## Timeout manager

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout manager backwards compatibility mode obsolete. If backwards compatibility mode was enabled these API's must be removed.

## Account aliases

Account aliases are enforced by default and `transport.UseAccountAliasesInsteadOfConnectionStrings()` is deprecated. See [Configuration API](/transports/azure-storage-queues/configuration.md#connection-strings-using-aliases-for-connection-strings-to-storage-accounts).

## Using clients

Queue, Blob and Table clients are the recommended way to configure the transport instead of using connection strings. Connections strings are still supported but will be removed in the future versions. See [Configuration API](/transports/azure-storage-queues/configuration.md#configuration-api).

## Transport connection string

The connection string set via `transport.ConnectionString()` is no longer mandatory and is only needed when no clients for queue, blob or table is configured.