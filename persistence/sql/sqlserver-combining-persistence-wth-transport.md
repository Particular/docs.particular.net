---
title: Combining MS SQL Server Persistence and Transport
component: SqlPersistence
reviewed: 2024-06-10
related:
  - persistence/sql/dialect-mysql
  - transports/sql
---

## Connection behavior

When combining [SQL Server transport](/transports/sql) and [SQL persistence using the Sql dialect](/persistence/sql), the connection behaves differently based on whether the [Outbox](/nservicebus/outbox/) is enabled or disabled. This influences where the saga data is stored.

### With Outbox

SQL Transport<br/>TransactionMode | Connection sharing | Saga location
:-:|:-:|:-:
TransactionScope | Not supported | N/A
AtomicSendsWithReceive | Not supported | N/A
ReceiveOnly | Connection sharing via SQL Transport storage context | Persistence DB
None | Not supported | N/A

When using the [outbox](/nservicebus/outbox/), SQL Persistence always opens its own connection.

### Without Outbox

SQL Transport<br/>TransactionMode | Connection sharing | Saga location
:-:|:-:|:-:
TransactionScope | SQL Transport transaction is promoted to distributed transaction | Persistence DB <sup>1</sup>
SendsAtomicWithReceive | SQL Transport uses isolated transaction for send and receive | Transport DB
ReceiveOnly | SQL Transport uses isolated transaction for receive | Transport DB
None | No transactions | Persistence DB

<sup>1</sup> - Requires .NET Framework, or .NET 8 [`ImplicitDistributedTransactions`](https://learn.microsoft.com/en-us/dotnet/api/system.transactions.transactionmanager.implicitdistributedtransactions?view=net-8.0) set to `true`.

#### Explicitly opting out of connection sharing when not using the Outbox

When an endpoint uses SQL Persistence combined with the SQL Server Transport with [Outbox](/nservicebus/outbox/) disabled, the persistence uses the connection and transaction context established by the transport when accessing saga data. This behavior ensures *exactly-once* message processing behavior as the state change of the saga is committed atomically while consuming the message that triggered it.

In order to force using a separate connection use the following API:

snippet: MsSqlDoNotShareConnection

> [!WARNING]
> When opting out of connection sharing with Outbox disabled, NServiceBus does not guarantee *exactly-once* message processing behavior which means that saga message handling logic might be called multiple times for a single incoming message in case the previous processing attempts failed just before consuming the message.


