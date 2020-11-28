---
title: Azure Storage Queues Transport Upgrade Version 6 to 7
summary: Instructions on how to upgrade the Azure Storage Queues transport from version 6 to 7.
reviewed: 2019-12-02
component: ASQ
related:
- transports/azure-storage-queues
- nservicebus/upgrades/5to6
redirects:
 - nservicebus/upgrades/asq-6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## New Configuration API

In versions 6 and below, the Azure Storage Queues transport was configured using an XML configuration section called `AzureStorageQueueTransportConfiguration`. This section has been removed in favor of a more granular, code-based configuration API.

The new configuration API is accessible through extension methods on the `UseTransport<AzureStorageQueueTransport>()` extension point in the endpoint configuration. See also [Azure Storage Queues Configuration](/transports/azure-storage-queues/configuration.md).

snippet: 6to7AzureStorageQueueTransportWithAzure


### To continue reading from app.config

Add the following to the app.config:

snippet: 6to7ConnectionStringFromConfigXml

Then read from app.config and pass the value to the transport configuration:

snippet: 6to7ConnectionStringFromConfig


### Setting the configuration values via API

Setting the configuration values can now be done via API in the following way:

 * [ConnectionString](/transports/azure-storage-queues/configuration.md#configuration-parameters-connectionstring)
 * [BatchSize](/transports/azure-storage-queues/configuration.md#configuration-parameters-batchsize)
 * [MaximumWaitTimeWhenIdle](/transports/azure-storage-queues/configuration.md#configuration-parameters-maximumwaittimewhenidle)
 * [DegreeOfReceiveParallelism](/transports/azure-storage-queues/configuration.md?version=asq_7#configuration-parameters-degreeofreceiveparallelism)
 * [PeekInterval](/transports/azure-storage-queues/configuration.md#configuration-parameters-peekinterval)
 * [MessageInvisibleTime](/transports/azure-storage-queues/configuration.md#configuration-parameters-messageinvisibletime)

These values can be set using corresponding extension methods:

snippet: AzureStorageQueueConfigCodeOnly


### PurgeOnStartup

The `PurgeOnStartup` setting now can be set on `EndpointConfiguration` using an extension method.

snippet: AzureStorageQueuePurgeOnStartup


### DefaultQueuePerInstance

The `DefaultQueuePerInstance` setting is deprecated.


### Default value changes

The default values of the following settings have been changed:

 * [ConnectionString](/transports/azure-storage-queues/configuration.md#configuration-parameters-connectionstring), which had a default value of `UseDevelopmentStorage=true`, was removed and became mandatory.
 * [BatchSize](/transports/azure-storage-queues/configuration.md#configuration-parameters-batchsize) changed from 10 to 32.


## Serialization

In previous versions of the Azure Storage Queues transport, change the default `SerializationDefinition` to `JsonSerializer`.

In version 6 of NServiceBus, transports no longer have the ability to manipulate serialization. To preserve backward compatibility and ensure that message payloads are small, setting JSON serialization should now be done on the endpoint configuration level.

snippet: 6to7-serializer-definition


## API Changes

In version 7, the public API has been reduced. As a result, multiple classes that used to be public in version 6 and below were marked as obsolete with a comment:

> This class served only internal purposes without providing any extensibility point and as such was removed from the public API. For more information, refer to the documentation.

If code exists that depends on classes that were deprecated with the above message, and it is not clear how to update it, [contact support](https://particular.net/contactus) to get help in resolving that issue.
