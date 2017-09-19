---
title: Azure Storage Queues Transport Upgrade Version 7 to 8
summary: Instructions on how to upgrade Azure Storage Queues Transport Version 7 to 8.
reviewed: 2017-09-18
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

 * Replacing invalid characters
 * Lowering the case
 * Shortening queue names exceeding the maximum allowed queue name length, using [SHA1](https://msdn.microsoft.com/en-us/library/system.security.cryptography.sha1.aspx) or [MD5](https://msdn.microsoft.com/en-us/library/system.security.cryptography.md5.aspx)

In Versions 8 and above, the transport is no longer performing sanitization by default. Instead, sanitization logic can be [registered](/transports/azure-storage-queues/sanitization.md).

snippet: AzureStorageQueueSanitization


## Serialization is mandatory

In previous versions, the transport was setting the default serialization. In version 8 and above, the transport is no longer sets the default serialization. Instead, it should be configured. 

For backwards compatibility, `NServiceBus.Newtonsoft.Json` serializer should be used.

snippet: AzureStorageQueueSerialization
