---
title: Service Fabric Persistence
reviewed: 2017-03-17
component: ServiceFabricPersistence
---

A persister built on top of [Service Fabric Reliable Collections](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-reliable-collections).


## Usage

Add a NuGet package reference to `NServiceBus.Persistence.ServiceFabric`. Configure the persistence technology using the following configuration API.

snippet: ServiceFabricPersistenceConfiguration

## Limitations

### Storage types

Currently saga and outbox storage types are supported. For gateway deduplication, timeouts or subscriptions use either the native capability of the transport of choice or another persistence like [Azure Storage Persistence](/nservicebus/azure-storage-persistence/index.md).

### Viewing the data

Currently Service Fabric does not provide capabilities to view the data that is stored inside reliable collections. The data has to be programatically accessed and exposed over customized infrastructure.