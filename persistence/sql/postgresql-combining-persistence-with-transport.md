---
title: Combining PostgreSQL Persistence and Transport
component: SqlPersistence
reviewed: 2024-06-14
related:
  - persistence/sql/dialect-postgresql
  - transports/postgresql
---

When [SQL persistence using the PostgreSQL dialect](/persistence/sql/dialect-postgresql.md) is used in combination with the [PostgreSQL transport](/transports/postgresql), the location of saga data depends on whether the [outbox](/nservicebus/outbox/) is enabled and the transaction mode.

## Outbox disabled

PostgreSQL Transport<br/>TransactionMode | Connection sharing | Saga data location
:-:|:-:|:-:
SendsAtomicWithReceive | PostgreSQL Transport uses isolated transaction for send and receive | Transport DB
ReceiveOnly | PostgreSQL Transport uses isolated transaction for receive | Transport DB
None | No transactions | Persistence DB

When the outbox is disabled, the persistence uses the connection and transaction context established by the transport when accessing saga data. This behavior ensures *exactly-once* message processing, as the state change of a saga is committed atomically with the consumption of the message that caused it.

partial: Connection

## Outbox enabled

PostgreSQL Transport<br/>TransactionMode | Connection sharing | Saga data location
:-:|:-:|:-:
SendsAtomicWithReceive | Not supported | N/A
ReceiveOnly | via PostgreSQL Transport storage context | Persistence DB
None | Not supported | N/A
