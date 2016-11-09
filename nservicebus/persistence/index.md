---
title: Persistence In NServiceBus
summary: Features of NServiceBus requiring persistence include timeouts, sagas, and subscription storage.
reviewed: 2016-10-26
redirects:
 - nservicebus/persistence-in-nservicebus
---

Various features require persistence.


## Storage Types

 * [Gateway Deduplication](/nservicebus/gateway/)
 * [Sagas](/nservicebus/sagas/)
 * [Subscriptions](/nservicebus/messaging/publish-subscribe/)
 * [Timeouts](/nservicebus/sagas/timeouts.md)
 * [Deferral](/nservicebus/messaging/delayed-delivery.md)
 * [Delayed Retries](/nservicebus/recoverability/#delayed-retries)
 * [Outbox](/nservicebus/outbox/)


## Available Persistences


### [In-Memory](in-memory.md)

A volatile RAM based storage mainly used for development purposes. Can also be used where the storage is not required to persist between process restarts.


### [RavenDB](/nservicebus/ravendb/)

Uses the [RavenDB document database](https://ravendb.net/) for storage.


### [NHibernate](/nservicebus/nhibernate/)

Uses custom [NHibernate](http://nhibernate.info/) to persist data to an ADO.net data store (e.g. SQL Server).


### [MSMQ](/nservicebus/msmq/subscription-persistence.md)

A subscription only storage on top of MSMQ.


### [Azure Storage](/nservicebus/azure-storage-persistence/)

Uses Azure Tables Storage for storage


### Community run Persistences

There are several community run Persistences that can be seen on the full list of [Extensions](/platform/extensions.md#persisters).
