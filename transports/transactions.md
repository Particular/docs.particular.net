---
title: Transport Transactions
summary: Supported transaction modes and their consistency guarantees
component: Core
versions: "[4,)"
redirects:
 - nservicebus/messaging/transactions
 - nservicebus/transports/transactions
reviewed: 2025-12-10
---

This article covers various levels of consistency guarantees with regard to:

 * receiving messages
 * updating user data
 * sending messages

NServiceBus provides four transaction modes that offer different consistency guarantees. Understanding these modes is essential for building reliable message-based systems.

### Key concepts

**[Atomicity](/nservicebus/concepts/glossary.md#atomicity)**: Ensures that operations either all succeed together or all fail together. For example, when processing a message, atomicity guarantees that receiving the message, updating the database, and sending outgoing messages are treated as a single unit of work.

**[Consistency](/nservicebus/concepts/glossary.md#consistency)**: Refers to how quickly data becomes visible across different parts of the system. *Immediate consistency* means data is available right away, while *eventual consistency* means data may not be immediately queryable but will become available after a short delay.

**[Idempotency](/nservicebus/concepts/glossary.md#idempotence)**: The ability to process the same message multiple times without causing unintended side effects. An idempotent handler produces the same business outcome whether it processes a message once or multiple times.

**[Ghost message](/nservicebus/concepts/glossary.md#ghost-message)**: A message that is sent to downstream systems, but the corresponding business data is never committed. This can occur when a message is successfully sent but the database transaction fails afterward.

**[Zombie record](/nservicebus/concepts/glossary.md#zombie-record)**: Business data that is stored in the database, but the corresponding messages are never sent to notify other parts of the system. This leaves "orphaned" data that other components don't know about.

> [!NOTE]
> This article focuses on coordination and failure handling across message and data operations. It does not discuss transaction isolation levels, which only apply to database operations themselves.

## Transactions

NServiceBus offers four transaction modes that provide different levels of guarantees when processing messages. Each mode represents a trade-off between consistency, reliability, and complexity:

1. **Transaction scope (Distributed transaction)** - Provides the strongest guarantees using distributed transactions across the transport and database
2. **Sends atomic with Receive** - Ensures outgoing messages are sent atomically with the receive operation
3. **Receive Only** - Guarantees the message won't be lost from the queue until successfully processed
4. **Unreliable (Transactions Disabled)** - Provides no transactional guarantees for maximum performance

The availability of each mode depends on the capabilities of the selected transport.

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

> [!NOTE]
> This mode requires the selected storage to support participating in distributed transactions.

#### Consistency guarantees

When the `TransportTransactionMode` is set to `TransactionScope`, handlers execute inside a `System.Transactions.TransactionScope` created by the transport. All data updates and queue operations are committed or rolled back together as a single atomic operation.

> [!WARNING]
> Not all persisters guarantee _immediately consistency_ on a single or full clusters when a distributed transaction is committed and only guarantee _eventual consistency_. Review a persister its transaction consistency guarantees to ensure the system consistency behaves as expected regarding distributed transactions, and (global) clustering/replication consistency.

### Transport transaction - Sends atomic with Receive

Some transports support enlisting outgoing operations in the current receive transaction. This ensures that messages are only sent to downstream endpoints when the receive operation completes successfully, preventing ghost messages during retries.

Use the following code to enable this mode:

snippet: TransportTransactionAtomicSendsWithReceive

#### Consistency guarantees

This mode provides the same consistency guarantees as *Receive Only* mode, with an important addition: it prevents ghost messages. Since all outgoing operations are committed atomically with the receive operation, messages are never sent if the handler fails and needs to be retried.

### Transport transaction - Receive Only

In this mode, the receive operation is wrapped in the transport's native transaction. The message is not permanently deleted from the queue until at least one processing attempt completes successfully. If processing fails, the message remains in the queue and will be [retried](/nservicebus/recoverability/).

> [!NOTE]
> [Sends and Publishes are batched](/nservicebus/messaging/batched-dispatch.md) and only transmitted after all handlers successfully complete. Messages that must be sent immediately should use the [immediate dispatch option](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately), which bypasses batching.

Use the following code to enable this mode:

snippet: TransportTransactionReceiveOnly

#### Consistency guarantees

This mode does not provide atomicity between the receive operation and other operations (database updates or sending messages). This can result in:

 * **Partial updates** - Some handlers succeed in updating data while others fail
 * **Partial sends** - Some messages are sent while others are not
 * **Ghost messages** - Messages are sent successfully, but subsequent database operations fail
 * **Zombie records** - Data is stored successfully, but outgoing messages fail to send

Additionally, handlers may be invoked multiple times for the same message due to retries. All handlers must be idempotent to ensure consistent business outcomes when processing the same message multiple times.

The [Outbox feature](#outbox) can handle idempotency at the infrastructure level, eliminating the need to design handlers for idempotency manually.

### Unreliable (Transactions Disabled)

This mode disables all transactional behavior and should only be used when message loss is acceptable. It may be appropriate for scenarios where messages become outdated quickly, such as sending sensor readings at regular intervals.

> [!CAUTION]
> In this mode, messages are **permanently lost** when encountering a critical failure such as a system or endpoint crash.

snippet: TransactionsDisable

> [!NOTE]
> The transport does not wrap the receive operation in any transaction. Messages that fail to process are moved directly to the error queue. There are no retries and no guarantees about message delivery.

## Outbox

The [Outbox](/nservicebus/outbox) feature provides exactly-once message processing semantics without requiring distributed transactions. It enables *Transport transaction* modes to achieve the same guarantees as *Transaction scope* mode, while avoiding the complexity and infrastructure requirements of distributed transactions.

> [!NOTE]
> The Outbox data must be stored in the same database as business data to achieve exactly-once delivery guarantees.

### How the Outbox works

When the Outbox is enabled, outgoing messages are not sent immediately. Instead:

1. The incoming message is processed and business data is updated
2. Outgoing messages are stored in an outbox table in the same database transaction as the business data
3. The database transaction commits, ensuring business data and outbox messages are saved atomically
4. After the transaction completes, the outgoing messages are dispatched from the outbox
5. Once dispatched, the outbox record is marked as complete

This approach ensures that message handling succeeds exactly once. Even if the message is processed multiple times due to retries, the outbox guarantees that outgoing messages maintain the same message ID. Receiving endpoints can deduplicate based on the message ID to ensure consistent processing, eliminating the need to design handlers for idempotency manually.

## Avoiding partial updates

When using transaction modes other than [Transaction scope](#transaction-modes-transaction-scope-distributed-transaction), there is a risk of partial updates. One handler might successfully update its data while another handler fails, leaving the system in an inconsistent state.

To prevent partial updates, NServiceBus can wrap all handlers in a `TransactionScope` that treats them as a single unit of work. This ensures that either all handlers succeed together or all fail together.

snippet: TransactionsWrapHandlersExecutionInATransactionScope

> [!NOTE]
> This requires that all data stores used by handlers support distributed transactions (e.g. SQL Server), including the saga store when using sagas.

> [!WARNING]
> This may escalate to a full distributed transaction if handlers update data in different databases.

> [!NOTE]
> This API must not be used when the transport is already running in *Transaction scope* mode.


partial: scope-options
