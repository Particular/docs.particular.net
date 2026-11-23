---
title: Persistence
summary: NServiceBus features requiring persistence include sagas, the outbox, subscription storage, and timeouts.
reviewed: 2026-06-01
redirects:
- nservicebus/persistence-in-nservicebus
- nservicebus/persistence
---

NServiceBus uses persistence to store saga state, enable the outbox, and support certain transport features.

## Features that require persistence

- [Sagas](/nservicebus/sagas/)
- [Outbox](/nservicebus/outbox/)
- [Subscriptions](/nservicebus/messaging/publish-subscribe/) (storage required if the transport does not support native publish-subscribe)
- Delayed delivery (storage required if the transport does not support native delayed delivery)
  - [Saga timeouts](/nservicebus/sagas/timeouts.md)
  - [Delayed retries](/nservicebus/recoverability/#delayed-retries)
  - [Deferral](/nservicebus/messaging/delayed-delivery.md)
- [Gateway deduplication](/nservicebus/gateway/)

## Selecting a persister

It can be a challenge to decide whether or not a persister is needed and which one is the best option for a specific scenario. See the [guide to selecting a persister](selecting.md) for help in making that decision.

## Supported persisters

- [SQL](/persistence/sql/)
  - [Microsoft SQL Server](/persistence/sql/dialect-mssql.md)
  - [MySQL](/persistence/sql/dialect-mysql.md)
  - [PostgreSQL](/persistence/sql/dialect-postgresql.md)
  - [Oracle](/persistence/sql/dialect-oracle.md)
  - Amazon Aurora (with MySQL or PostgreSQL) and Amazon RDS
- [Cosmos DB](/persistence/cosmosdb/)
- [DynamoDB](/persistence/dynamodb/)
- [MongoDB](/persistence/mongodb/)
- [Azure Table](/persistence/azure-table/)
- [NHibernate](/persistence/nhibernate/)
- [RavenDB](/persistence/ravendb/)
- [MSMQ](/persistence/msmq/)
- [Learning](/persistence/learning/)
- [Non-Durable](/persistence/non-durable/)

## Persistence at a glance

The main page for each persistence library includes a **Persistence at a glance** section that calls out some of the important differences between each option. This section describes what each item means and why it is important.

- **Supported storage types**: The [features](#features-that-require-persistence) that the library supports.
  - May include sagas, the outbox, and subscriptions.
  - Support for timeouts includes delayed retries and message deferral as well.
  - Gateway deduplication is not covered, as the [gateway component](/nservicebus/gateway/) is a separate package from NServiceBus and has its own persistence packages.
- **Transactions**: Describes how the persister keeps changes to saga data, outbox data, and business data consistent within message handlers. Persisters with this capability expose the transaction or session through the `SynchronizedStorageSession` property of `IMessageHandlerContext`.
- **Concurrency control**: Describes how the persistence behaves when multiple message handlers attempt to update saga data simultaneously. Packages that enable pessimistic concurrency offer [better performance for sagas implementing a scatter-gather pattern](https://particular.net/blog/optimizations-to-scatter-gather-sagas).
- **Scripted deployment**: Describes how storage prerequisites (such as table schemas) can be deployed as part of a DevOps process.
- **Installers**: Describes whether the persistence can create storage prerequisites (such as table schemas) at runtime via the [installers feature](/nservicebus/operations/installers.md) for a smoother development-time experience.
