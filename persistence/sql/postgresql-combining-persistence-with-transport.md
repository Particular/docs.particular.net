---
title: Combining PostgreSQL Persistence and Transport
component: SqlPersistence
reviewed: 2024-06-14
related:
  - persistence/sql/dialect-postgresql
  - transports/postgresql
---

When using [SQL persistence using the PostgreSQL dialect](/persistence/sql/dialect-postgresql.md) in combination with the [PostgreSQL transport](/transports/postgresql), the location of saga data depends both on whether the [outbox](/nservicebus/outbox/) is enabled and on the transaction mode.

## Outbox disabled

PostgreSQL Transport<br/>TransactionMode | Connection sharing | Saga data location
:-:|:-:|:-:
SendsAtomicWithReceive | isolated transaction for send and receive | Transport DB
ReceiveOnly | isolated transaction for receive | Transport DB
None | No transactions | Persistence DB

With the outbox disabled, the persistence uses the connection and transaction context established by the transport to access saga data. This behavior ensures *exactly-once* message processing, since the state change of a saga is committed atomically with the consumption of the message that caused it.

partial: Connection

## Outbox enabled

PostgreSQL Transport<br/>TransactionMode | Connection sharing | Saga data location
:-:|:-:|:-:
SendsAtomicWithReceive | Not supported | N/A
ReceiveOnly | via Transport storage context | Persistence DB
None | Not supported | N/A
