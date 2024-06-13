---
title: Transport Transactions
summary: Supported transaction modes and their consistency guarantees
component: Core
versions: "[4,)"
redirects:
 - nservicebus/messaging/transactions
 - nservicebus/transports/transactions
reviewed: 2024-02-19
---

This article covers various levels of consistency guarantees with regard to:

 * receiving messages
 * updating user data
 * sending messages

It does not discuss the transaction isolation aspect. Transaction isolation applies only to the process of updating the user data, it does not affect the overall coordination and failure handling.


## Transactions

Four levels of guarantees with regard to message processing are offered. A level's availability depends on the selected transport.


### Transaction levels supported by NServiceBus transports

The implementation details for each transport are discussed in the dedicated documentation sections. They can be accessed by clicking the links with the transport name in the following table:

partial: matrix

> [!WARNING]
> When combining [SQL Server transport](/transports/sql/) and [SQL persistence with the Microsoft SQL Server dialect](/persistence/sql/dialect-mssql.md), be aware of the different [connection behaviors](/persistence/sql/sqlserver-combining-persistence-wth-transport.md).

### Transaction scope (Distributed transaction)

In this mode the transport receive operation is wrapped in a [`TransactionScope`](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionscope). Other operations inside this scope, both sending messages and manipulating data, are guaranteed to be executed (eventually) as a whole or rolled back as a whole.

If required, the transaction is escalated to a distributed transaction (following a two-phase commit protocol coordinated by [MSDTC](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ms684146(v=vs.85))) if both the transport and the persistence support it. A fully distributed transaction is not always required, for example when using [SQL Server transport](/transports/sql/) with [SQL persistence](/persistence/sql/), both using the same database connection string. In this case, the ADO.NET driver guarantees that everything happens inside a single database transaction and ACID guarantees are held for the whole processing.

> [!NOTE]
> MSMQ will escalate to a distributed transaction right away since it doesn't support promotable transaction enlistments.

*Transaction scope* mode is enabled by default for the transports that support it (i.e. MSMQ and SQL Server transport). It can be enabled explicitly with the following code:

snippet: TransportTransactionScope

include: mssql-dtc-warning


#### Atomicity and consistency guarantees

When the `TransportTransactionMode` is set to `TransactionScope`, handlers will execute inside a `System.Transactions.TransactionScope` created by the transport. This means that all the data updates and queue operations are committed or rolled back.

**A distributed transaction between the queueing system and the persistent storage guarantees _atomic commits_ but guarantees only _eventual consistency_.**

Consider a system using MSMQ transport and RavenDB persistence implementing the following message exchange scenario with a saga that models a simple order lifecycle:

 1. `OrderSaga` receives a `StartOrder` message
 1. A new `OrderSagaData` instance is created and stored in RavenDB.
 1. `OrderSaga` sends `VerifyPayment` message to `PaymentService`.
 1. NServiceBus completes the distributed transaction and the DTC instructs MSMQ and RavenDB resource managers to commit their local transactions.
 1. The `StartOrder` message is removed from the input queue and `VerifyPayment` is immediately sent to `PaymentService`.
 1. RavenDB acknowledges the transaction commit and begins writing `OrderSagaData` to disk.
 1. `PaymentService` receives a `VerifyPayment` message and immediately responds with a `CompleteOrder` message to the originating `OrderSaga`.
 1. `OrderSaga` receives the `CompleteOrder` message and attempts to complete the saga.
 1. `OrderSaga` queries RavenDB to find the `OrderSagaData` instance to complete.
 1. RavenDB has not finished writing `OrderSagaData` to disk and returns an empty result set.
 1. `OrderSaga` fails to complete.

In the example above the `TransactionScope` guarantees atomicity for the `OrderSaga`: consuming the incoming `StartOrder` message, storing `OrderSagaData` in RavenDB and sending the outgoing `VerifyPayment` message are _committed_ as one atomic operation. The saga data may not be immediately available for reading even though the incoming message has already been processed. `OrderSaga` is thus only _eventually consistent_. The `CompleteOrder` message needs to be [retried](/nservicebus/recoverability/) until RavenDB successfully returns an `OrderSagaData` instance.

> [!NOTE]
> This mode requires the selected storage to support participating in distributed transactions.

### Transport transaction - Sends atomic with Receive

Some transports support enlisting outgoing operations in the current receive transaction. This prevents messages from being sent to downstream endpoints during retries.

Use the following code to use this mode:

snippet: TransportTransactionAtomicSendsWithReceive

#### Consistency guarantees

This mode has the same consistency guarantees as the *Receive Only* mode, but additionally, it prevents the occurrence of [ghost messages](/nservicebus/concepts/glossary.md#ghost-message) since all outgoing operations are atomic with the ongoing receive operation.

### Transport transaction - Receive Only

In this mode, the receive operation is wrapped in a transport's native transaction. This mode guarantees that the message is not permanently deleted from the incoming queue until at least one processing attempt (including storing user data and sending out messages) is finished successfully. See also [recoverability](/nservicebus/recoverability/) for more details on retries.

> [!NOTE]
> [Sends and Publishes are batched](/nservicebus/messaging/batched-dispatch.md) and only transmitted until all handlers are successfully invoked.
Messages that are required to be sent immediately should use the [immediate dispatch option](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately) which bypasses batching.

Use the following code to use this mode:

snippet: TransportTransactionReceiveOnly


#### Consistency guarantees

In this mode some (or all) handlers might get invoked multiple times and partial results might be visible:

 * partial updates - where one handler succeeded in updating its data but the other didn't
 * partial sends - where some of the messages have been sent but others not

When using this mode all handlers must be [idempotent](/nservicebus/concepts/glossary.md#idempotence), i.e. the result needs to be consistent from a business perspective even when the message is processed more than once.

See the `Outbox` section below for details on how NServiceBus can handle idempotency at the infrastructure level.


### Unreliable (Transactions Disabled)

Disabling transactions is generally not recommended, because it might lead to message loss. It might be considered if losing some messages is not problematic and if the messages get outdated quickly, e.g. when sending readings from sensors at regular intervals.

> [!CAUTION]
> In this mode, when encountering a critical failure such as a system or endpoint crash, the message is **permanently lost**.

snippet: TransactionsDisable

> [!NOTE]
> In this mode the transport doesn't wrap the receive operation in any kind of transaction. Should the message fail to process it will be moved straight to the error queue.


## Outbox

The [Outbox](/nservicebus/outbox) feature provides idempotency at the infrastructure level and allows running in *transport transaction* mode while still getting the same semantics as *Transaction scope* mode.

> [!NOTE]
> Outbox data needs to be stored in the same database as business data to achieve the `exactly-once delivery`.

When using the Outbox, any messages resulting from processing a given received message are not sent immediately but rather stored in the persistence database and pushed out after the handling logic is done. This mechanism ensures that the handling logic can only succeed once so there is no need to design for idempotency.


## Avoiding partial updates

In transaction modes lower than [TransactionScope](#transactions-transaction-scope-distributed-transaction) there is a risk of partial updates because one handler might succeed in updating business data while another handler fails. To avoid this configure NServiceBus to wrap all handlers in a `TransactionScope` that will act as a unit of work and make sure that there are no partial updates. Use the following code to enable a wrapping scope:

snippet: TransactionsWrapHandlersExecutionInATransactionScope

> [!NOTE]
> This requires that all the data stores used by the handler support enlisting in a distributed transaction (e.g. SQL Server), including the saga store when using sagas.

> [!WARNING]
> This might escalate to a distributed transaction if data in different databases are updated.

> [!NOTE]
> This API must not be used in combination with transports running in a *transaction scope* mode.


partial: scope-options
