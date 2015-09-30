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

WARN: Because there is no clustering / replication happening between NServiceBus instances, when a process is down, all the information stored in the InMemory persistence of that process is lost.

NOTE: The [Second Level Retry (SLR)](/nservicebus/errors/automatic-retries#second-level-retries) mechanism uses the deferred delivery (also known as *timeouts*) feature internally. As SLR is enabled by default, using InMemory persistence means you are risking losing messages that have failed processing and are waiting for another retry attempt. Use InMemory persistence only is scenarios where it is OK to lose messages.

### [RavenDB](/nservicebus/ravendb/)

Uses the [RavenDB document database](http://ravendb.net/) for storage.

### [NHibernate](/nservicebus/nhibernate/)

Uses custom [NHibernate](http://nhibernate.info/) to persister data to an ADO.net data store (eg SQL Server).


### [MSMQ](/nservicebus/msmq/subscription-persistence.md)

A subscription only storage on top of MSMQ.


### [Azure Storage](/nservicebus/azure/azure-storage-persistence.md)

Uses Azure Tables Storage for storage


### Community run Persistences

There are several community run Persistences that can be seen on the full list of [Extensions](/platform/extensions.md#persisters).
