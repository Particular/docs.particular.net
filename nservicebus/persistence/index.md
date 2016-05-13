---
title: Persistence In NServiceBus
summary: Features of NServiceBus requiring persistence include timeouts, sagas, and subscription storage.
tags: []
redirects:
- nservicebus/persistence-in-nservicebus
---

Various features of NServiceBus require persistence.


## Storage Types

 * [Gateway Deduplication](/nservicebus/gateway/)
 * [Sagas](/nservicebus/sagas/)
 * [Subscriptions](/nservicebus/sagas/)
 * [Timeouts](/nservicebus/sagas/#timeouts) (including message deferral and SLR)
 * [Outbox](/nservicebus/outbox/)


## Available Persistences


### [In-Memory](in-memory.md)

A volatile RAM based storage mainly used for development purposes. Can also be used where the storage is not required to persist between process restarts.

### [RavenDB](/nservicebus/ravendb/)

Uses the [RavenDB document database](http://ravendb.net/) for storage.

### [NHibernate](/nservicebus/nhibernate/)

Uses custom [NHibernate](http://nhibernate.info/) to persist data to an ADO.net data store (eg SQL Server).


### [MSMQ](/nservicebus/msmq/subscription-persistence.md)

A subscription only storage on top of MSMQ.


### [Azure Storage](/nservicebus/azure-storage-persistence/)

Uses Azure Tables Storage for storage


### Community run Persistences

There are several community run Persistences that can be seen on the full list of [Extensions](/platform/extensions.md#persisters).
