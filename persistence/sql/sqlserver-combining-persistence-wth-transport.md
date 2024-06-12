---
title: Combining MS SQL Server Persistence and Transport
component: SqlPersistence
reviewed: 2024-06-10
related:
  - persistence/sql/dialect-mysql
  - transports/sql
---

## Connection behavior

When combining SQL Server transport and persistence using the Sql dialect, the connection behaves differently based on whether the [Outbox](/nservicebus/outbox/) is used or not. This influences where the saga data is stored.

### Without Outbox

SQL Transport<br/>TransactionMode | SQL Persistence<br/>with Sql dialect | Connection sharing | Saga location
:-:|:-:|:-:|:-:
TransactionScope |  ✅| SQLT transaction is promoted to distributed transaction | Persistence DB <sup>1</sup>
SendsAtomicWithReceive |  ✅| SQLT uses isolated transaction for send and receive | Transport DB
ReceiveOnly |  ✅| SQLT uses isolated transaction for receive | Transport DB
None |  ✅| No transactions | Persistence DB

<sup>1</sup> - Requires .NET Framework, or .NET 8 with `System.Transactions.TransactionManager.ImplicitDistributedTransactions = true;`

#### Explicitly opting out of connection sharing when not using the Outbox

When an endpoint uses SQL Persistence combined with the SQL Server Transport without the [Outbox](/nservicebus/outbox/), the persistence uses the connection and transaction context established by the transport when accessing saga data. This behavior ensures *exactly-once* message processing behavior as the state change of the saga is committed atomically while consuming of the message that triggered it.

partial: Connection

### With Outbox

SQL Transport<br/>TransactionMode | SQL Persistence<br/>with Sql dialect | Connection sharing | Saga location
:-:|:-:|:-:|:-:
TransactionScope |  ✅|  Not supported | N/A
AtomicSendsWithReceive |  ✅| Not supported | N/A
ReceiveOnly |  ✅| Connection sharing via SQLT storage context | Persistence DB
None |  ✅| Not supported | N/A
