---
title: Transaction support
summary: The design and implementation details of PostgreSQL transport transaction support
reviewed: 2024-05-27
component: PostgreSqlTransport
---


The PostgreSQL transport supports the following [transport transaction modes](/transports/transactions.md):

* Transport transaction - Send atomic with receive
* Transport transaction - receive only
* Unreliable (transactions disabled)

`TransactionScope` mode is not supported because the implementation of `TransactionScope` in [npgsql](https://www.npgsql.org/index.html) (ADO.NET driver for PostgreSQL) [may lead to logical message loss](https://github.com/npgsql/npgsql/issues/5683) (an update to a database that is prepared and persisted but not actually applied to the data tables).

> [!NOTE]
> `Exactly once` message processing without distributed transactions can be achieved with any transport using the [Outbox](/nservicebus/outbox/) feature. It requires business and persistence data to share the storage mechanism but does not put any requirements on transport data storage. In addition, PostgreSQL transport supports the `exactly once` processing mode when using **SendsAtomicWithReceive** transaction mode [in conjunction with SQL Persistence](/persistence/sql/postgresql-combining-persistence-with-transport.md).

### Native transactions

In this mode, the message is received inside a native ADO.NET transaction. There are two available options within native transaction level:

 * **ReceiveOnly** - An input message is received using native transaction. The transaction is committed only when message processing succeeds.

> [!NOTE]
> This transaction is not shared outside of the message receiver and therefore there is a possibility of persistent side-effects when processing fails, i.e. [ghost messages](/nservicebus/concepts/glossary.md#ghost-message) might occur.

 * **SendsAtomicWithReceive** - This mode is similar to the `ReceiveOnly`, but the transaction is shared with sending operations. The message receive operation and any send or publish operations are committed atomically.

### Unreliable (transactions disabled)

In this mode, when a message is received it is immediately removed from the input queue. If processing fails the message is lost because the operation cannot be rolled back. Any other operation that is performed when processing the message is executed without a transaction and cannot be rolled back. This can lead to undesired side effects when message processing fails part way through.
