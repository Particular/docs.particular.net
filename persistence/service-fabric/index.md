---
title: Service Fabric Persistence
reviewed: 2020-11-26
component: ServiceFabricPersistence
related:
- samples/azure/azure-service-fabric-routing
---

A persister built on top of [Service Fabric Reliable Collections](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-reliable-collections).


## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Supported storage types    |Sagas, Outbox
|Transactions               |Yes
|Concurrency control        |Pessimistic concurrency via [exclusive locks](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-reliable-collections-transactions-locks#locks)
|Scripted deployment        |Not supported
|Installers                 |None


## Usage

Add a NuGet package reference to `NServiceBus.Persistence.ServiceFabric`. Configure the persistence technology using the following configuration API.

snippet: ServiceFabricPersistenceConfiguration

partial: timeout

## Limitations


### Storage types

Currently saga and outbox storage types are supported. For timeouts, subscriptions or gateway deduplication use either the native capability of the transport of choice or another persistence such as [Azure Table Persistence](/persistence/azure-table) or [non-durable persistence](/persistence/non-durable/).


### Viewing the data

Currently, Service Fabric does not provide capabilities to view the data that is stored inside reliable collections. The data has to be programmatically accessed and exposed over customized infrastructure.

Data should be backed up and restored to avoid data loss. See Service Fabric [backup and restore APIs](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-backup-restore) for additional information.
