---
title: Azure Storage Queues Transport Upgrade Version 7 to 8
summary: Migration instructions on how to upgrade Azure Storage Queues Transport from Version 7 to 8.
reviewed: 2019-09-26
component: ASQ
related:
- transports/azure-storage-queues
- nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---


## Sanitization

In previous versions, the transport was responsible for sanitization of the queue names. That included:

 * Replacing invalid characters
 * Lowering the case
 * Shortening queue names exceeding the maximum allowed queue name length, using [SHA1](https://msdn.microsoft.com/en-us/library/system.security.cryptography.sha1.aspx) or [MD5](https://msdn.microsoft.com/en-us/library/system.security.cryptography.md5.aspx)

In Versions 8 and above, the transport is no longer performing sanitization by default. Instead, sanitization logic must be [registered](/transports/azure-storage-queues/sanitization.md).

snippet: azure-storage-queue-sanitization


## Serialization is mandatory

In Versions 7 and below, the transport was setting the default serialization. In Versions 8 and above, the transport no longer sets the default serialization. Instead, it must be configured. 

For backwards compatibility with the previous default serialization, `NServiceBus.Newtonsoft.Json` serializer must be used.

snippet: AzureStorageQueueSerialization
