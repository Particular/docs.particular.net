---
title: Persistence
summary: Features of NServiceBus requiring persistence include timeouts, sagas, and subscription storage.
reviewed: 2021-01-04
redirects:
- nservicebus/persistence-in-nservicebus
- nservicebus/persistence
---

NServiceBus needs to store data for various purposes, such as persisting the state of sagas, enabling the outbox, and for certain other transport features.


## Features that require persistence

 * [Sagas](/nservicebus/sagas/)
 * [Outbox](/nservicebus/outbox/)
 * [Subscriptions](/nservicebus/messaging/publish-subscribe/) (Storage required if transport does not support native publish-subscribe)
 * [Timeouts](/nservicebus/sagas/timeouts.md) (Storage required if the transport does not support native delayed delivery)
   * [Delayed Retries](/nservicebus/recoverability/#delayed-retries) (Storage required if the transport does not support native delayed delivery)
   * [Deferral](/nservicebus/messaging/delayed-delivery.md) (Storage required if the transport does not support native delayed delivery)
 * [Gateway Deduplication](/nservicebus/gateway/)

## Selecting a persister

It can be a challenge to decide whether or not a persister is needed and which one is the best option for a specific scenario. See the [guide to selecting a persister](selecting.md) for help in making that decision.


## Supported persisters

- [SQL](/persistence/sql/)
- [NHibernate](/persistence/nhibernate/)
- [Cosmos DB (preview)](/previews/cosmosdb/)
- [MongoDB](/persistence/mongodb/)
- [Azure Table](/persistence/azure-table/)
- [RavenDB](/persistence/ravendb/)
- [Service Fabric](/persistence/service-fabric/)
- [MSMQ](/persistence/msmq/)
- [Learning](/persistence/learning/)
- [Non-Durable](/persistence/non-durable/)


## Persistence at a glance

The main page for each persistence library includes a **Persistence at a glance** section that calls out some of the important differences between each option. This section describes what each item means and why it is important.

* **Supported storage types**: The [features](#features-that-require-persistence) that are supported by the library.
  * May include Sagas, Outbox, Subscriptions, and Timeouts.
  * Support for timeouts includes delayed retries and message deferral as well.
  * Gateway deduplication is not covered, as the [gateway component](/nservicebus/gateway/) is a separate package from NServiceBus and has its own persistence packages.
* **Transactions**: Describes how changes to saga and/or outbox data are kept consistent with each other and with changes to business data made in message handlers. Persistence that has this type of transactional capability will expose access to the transaction/session through the `SynchronizedStorageSession` property of `IMessageHandlerContext`.
* **Concurrency control**: Describes how the persistence behaves when multiple message handlers attempt to update saga data simultaneously. Packages that enable pessimistic concurrency offer [better performance for sagas implementing a scatter-gather pattern](https://particular.net/blog/optimizations-to-scatter-gather-sagas).
* **Scripted deployment**: Describes how storage prerequisites (such as table schema) can be deployed as part of a DevOps process.
* **Installers**: Describes whether the persistence can create storage prerequisites (such as table schema) at runtime via the [installers feature](/nservicebus/operations/installers.md) for a smoother development-time experience.
