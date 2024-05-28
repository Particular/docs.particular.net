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

`TransactionScope` mode is particularly useful as it enables `exactly once` message processing with distributed transactions. However, when transport, persistence, and business data are all stored in a single PostgreSQL catalog, it is possible to achieve `exactly-once` message delivery without distributed transactions.

> [!NOTE]
> `Exactly once` message processing without distributed transactions can be achieved with any transport using the [Outbox](/nservicebus/outbox/) feature. It requires business and persistence data to share the storage mechanism but does not put any requirements on transport data storage.


### Native transactions

In this mode, the message is received inside a native ADO.NET transaction. There are two available options within native transaction level:

 * **ReceiveOnly** - An input message is received using native transaction. The transaction is committed only when message processing succeeds.

> [!NOTE]
> This transaction is not shared outside of the message receiver. That means there is a possibility of persistent side-effects when processing fails, i.e. [ghost messages](/nservicebus/concepts/glossary.md#ghost-message) might occur.

 * **SendsAtomicWithReceive** - This mode is similar to the `ReceiveOnly`, but transaction is shared with sending operations. That means the message receive operation and any send or publish operations are committed atomically.


### Unreliable (transactions disabled)

In this mode, when a message is received it is immediately removed from the input queue. If processing fails the message is lost because the operation cannot be rolled back. Any other operation that is performed when processing the message is executed without a transaction and cannot be rolled back. This can lead to undesired side effects when message processing fails part way through.
