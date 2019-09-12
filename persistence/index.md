---
title: Persistence
summary: Features of NServiceBus requiring persistence include timeouts, sagas, and subscription storage.
reviewed: 2019-02-27
redirects:
- nservicebus/persistence-in-nservicebus
- nservicebus/persistence
---

NServiceBus needs to store data for various purposes, such as persisting the state of sagas, enabling the outbox, and for certain other transport features.


## Features that require persistence

 * [Sagas](/nservicebus/sagas/)
 * [Outbox](/nservicebus/outbox/)
 * [Subscriptions](/nservicebus/messaging/publish-subscribe/)
 * [Timeouts](/nservicebus/sagas/timeouts.md)
 * [Delayed Retries](/nservicebus/recoverability/#delayed-retries)
 * [Deferral](/nservicebus/messaging/delayed-delivery.md)
 * [Gateway Deduplication](/nservicebus/gateway/)

## Selecting a persister

It can be a challenge to decide whether or not a persister is needed and which one is the best option for a specific scenario. See the [guide to selecting a persister](selecting.md) for help in making that decision.


## Supported persisters

- [Learning](/persistence/learning/)
- [In-Memory](in-memory.md)
- [SQL](/persistence/sql/)
- [Azure Storage](/persistence/azure-storage/)
- [RavenDB](/persistence/ravendb/)
- [Service Fabric](/persistence/service-fabric/)
- [NHibernate](/persistence/nhibernate/)
- [MSMQ](/persistence/msmq/subscription.md)
- [MongoDB](/persistence/mongodb/)

## Community-maintained persisters

There are several community-maintained persisters which can be found in the full list of [extensions](/components#persisters).
