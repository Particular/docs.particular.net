---
title: Azure Storage Queues Transport Upgrade Version 6 to 7
summary: Instructions on how to upgrade Azure Storage Queues Transport Version 6 to 7.
reviewed: 2016-04-05
tags:
 - upgrade
 - migration
related:
- nservicebus/azure-storage-queues
- nservicebus/upgrades/5to6
---


## Azure Storage Queues Transport

### Serialization
In previous versions, the Azure Storage Queues Transport was changing the `SerializationDefinition` to `JsonSerializer`.
Because of the changes in the core of NServiceBus, transports have no longer ability to change this setting. To preserve backward compatibility and ensure that message payloads are small, setting a JSON serialization has to be done on the endpoint configuration level. 

snippet:6to7-serializer-definition