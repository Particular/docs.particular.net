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

## Timeout manager

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout manager backwards compatibility mode obsolete. If backwards compatibility mode was enabled these API's must be removed.

## Account aliases

Account aliases are enforced by default and `transport.UseAccountAliasesInsteadOfConnectionStrings()` is deprecated. See [Configuration API](/transports/azure-storage-queues/configuration.md#connection-strings-using-aliases-for-connection-strings-to-storage-accounts).

## Using clients

Queue, Blob and Table clients are the recommended way to configure the transport instead of using connection strings. Connections strings are still supported but will be removed in the future versions. See [Configuration API](/transports/azure-storage-queues/configuration.md#configuration-api).