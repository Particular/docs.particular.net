---
title: Service Fabric Persistence
reviewed: 2017-03-29
component: ServiceFabricPersistence
related: samples/azure/azure-service-fabric-routing
---

A persister built on top of [Service Fabric Reliable Collections](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-reliable-collections).


## Usage

Add a NuGet package reference to `NServiceBus.Persistence.ServiceFabric`. Configure the persistence technology using the following configuration API.

snippet: ServiceFabricPersistenceConfiguration


## Limitations


### Storage types

Currently saga and outbox storage types are supported. For timeouts, subscriptions or gateway deduplication use either the native capability of the transport of choice or another persistence such as [Azure Storage Persistence](/nservicebus/azure-storage-persistence) or [In-Memory persistence](/nservicebus/persistence/in-memory.md).


### Viewing the data

Currently, Service Fabric does not provide capabilities to view the data that is stored inside reliable collections. The data has to be programmatically accessed and exposed over customized infrastructure.

Data also should be backed up and restored to avoid data loss. See Service Fabric [backup and restore APIs](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-backup-restore).
