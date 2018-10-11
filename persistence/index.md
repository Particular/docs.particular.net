---
title: Persistence
summary: Features of NServiceBus requiring persistence include timeouts, sagas, and subscription storage.
reviewed: 2018-08-23
redirects:
- nservicebus/persistence-in-nservicebus
- nservicebus/persistence
---

Several NServiceBus features require data to be persisted to storage.


## Features that require persistence

 * [Gateway Deduplication](/nservicebus/gateway/)
 * [Sagas](/nservicebus/sagas/)
 * [Subscriptions](/nservicebus/messaging/publish-subscribe/)
 * [Timeouts](/nservicebus/sagas/timeouts.md)
 * [Deferral](/nservicebus/messaging/delayed-delivery.md)
 * [Delayed Retries](/nservicebus/recoverability/#delayed-retries)
 * [Outbox](/nservicebus/outbox/)


## Available persisters


### [Learning](/persistence/learning/)

include: learning-persistence-description


### [In-Memory](in-memory.md)

A volatile RAM-based storage intended for development purposes only. The in-memory persister can also be used where the storage is not required to persist between process restarts.


### [SQL](/persistence/sql/)

Uses [Json.NET](http://www.newtonsoft.com/json) to serialize data and store in a SQL database, such as SQL Server or MySQL.


### [Azure Storage](/persistence/azure-storage/)

Persists data to Azure Table storage.


### [RavenDB](/persistence/ravendb/)

Uses the [RavenDB document database](https://ravendb.net/) for storage.


### [Service Fabric](/persistence/service-fabric/)

A persister built on [Azure Service Fabric Reliable Collections](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-reliable-collections).


### [NHibernate](/persistence/nhibernate/)

Uses [NHibernate](http://nhibernate.info/) to persist data to an ADO.NET data store, such as SQL Server.


### [MSMQ](/persistence/msmq/subscription.md)

A subscription-only storage build on MSMQ.


### Community-maintained persisters

There are several community-maintained persisters that can be seen in the full list of [extensions](/components#persisters).
