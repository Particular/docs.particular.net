---
title: Combining PostgreSQL Persistence and Transport
component: SqlPersistence
reviewed: 2024-06-14
related:
  - persistence/sql/dialect-postgresql
  - transports/postgresql
---

## Connection behavior

When combining [PostgreSQL](/transports/postgresql) and [SQL persistence using the PostgreSQL dialect](/persistence/sql/dialect-postgresql.md), the connection behaves differently based on whether the [Outbox](/nservicebus/outbox/) is enabled or disabled. This influences where the saga data is stored.

### Without Outbox

PostgreSQL Transport<br/>TransactionMode | Connection sharing | Saga location
:-:|:-:|:-:
SendsAtomicWithReceive | PostgreSQL Transport uses isolated transaction for send and receive | Transport DB
ReceiveOnly | PostgreSQL Transport uses isolated transaction for receive | Transport DB
None | No transactions | Persistence DB

#### Explicitly opting out of connection sharing when not using the Outbox

When an endpoint uses PostgreSQL Persistence combined with the PostgreSQL Transport with [Outbox](/nservicebus/outbox/) disabled, the persistence uses the connection and transaction context established by the transport when accessing saga data. This behavior ensures *exactly-once* message processing behavior as the state change of the saga is committed atomically while consuming the message that triggered it.

partial: Connection

### With Outbox

PostgreSQL Transport<br/>TransactionMode | Connection sharing | Saga location
:-:|:-:|:-:
SendsAtomicWithReceive | Not supported | N/A
ReceiveOnly | Connection sharing via PostgreSQL Transport storage context | Persistence DB
None | Not supported | N/A
