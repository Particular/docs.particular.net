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
 * [Timeouts](/nservicebus/sagas/#timeouts)
 * [Outbox](/nservicebus/outbox/)

## Available Persistences

### [In-Memory](in-memory.md)

A volatile RAM based () storage mainly used for development purposes. Can also be used where the storage is not required to persist between process restarts.

### [RavenDB](/nservicebus/ravendb/)

Uses the [RavenDB document database](http://ravendb.net/) for storage.

### [NHibernate](/nservicebus/nhibernate/)

Uses custom [NHibernate](http://nhibernate.info/) to persister data to an ADO.net data store (eg SQL Server).

### [MSMQ](/nservicebus/msmq)

A subscription only storage.

NOTE: Storing your subscriptions in MSMQ is not suitable for scenarios where you need to scale the endpoint out. The reason is that the subscription queue cannot be shared among multiple endpoints. 

### Community run Persistences

There are several community run Persistences that can be seen on the full list of [Extensions](/platform/extensions.md#persisters).