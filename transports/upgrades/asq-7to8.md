---
title: Azure Storage Queues Transport Upgrade Version 7 to 8
summary: Instructions on how to upgrade Azure Storage Queues Transport Version 7 to 8.
reviewed: 2017-09-07
component: ASQ
related:
- transports/azure-storage-queues
- nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


## Sanitization

In previous versions, the transport was responsible for sanitization of the queue names. That included:
- Replacing invalid characters
- Lowering the case
- Shortening queue names exceeding the maximum allowed queue name length, using SHA1 or MD5

In version 8 and above, the transport is no longer performing sanitization by default. Instead, sanitization logic can be [registered](/transports/azure-storage-queues/sanitization.md).

snippet: AzureStorageQueueSanitization