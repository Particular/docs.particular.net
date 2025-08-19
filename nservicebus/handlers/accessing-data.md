---
title: Accessing and modifying data from a message handler
summary: How to access business data from a message handler and sync with message consumption and modifications to NServiceBus-controlled data.
component: Core
versions: '[6,)'
reviewed: 2024-01-05
related:
 - persistence/nhibernate/accessing-data
 - persistence/sql/accessing-data
 - persistence/mongodb
 - persistence/ravendb
 - persistence/service-fabric
 - persistence/cosmosdb
 - persistence/dynamodb
 - persistence/azure-table
---

In most cases, [handlers](/nservicebus/handlers/) are meant to modify the internal state of an application based on the received message. In any system, it is critical to ensure the state change is persisted exactly once. In a messaging system, this can be a challenge as exactly-once delivery is not guaranteed by any single queuing technology. NServiceBus provides several strategies to mitigate the risk of an inconsistent application state.

Accessing the application state can be achieved in a number of ways and should not be enforced or restricted by NServiceBus. The scenarios below provide guidance on several ways of accessing the application state by using the same data context as NServiceBus uses internally.

partial: transactionscope

### Synchronized storage session

A synchronized storage session is an NServiceBus implementation of the unit of work pattern. It provides a data access context that is shared by all handlers that process a given message. The state change is committed after the execution of message handlers, provided that there are no exceptions during processing. The synchronized storage session is accessible via the `IMessageHandlerContext`:

snippet: BusinessData-SynchronizedStorageSession

The synchronized storage session feature is supported by most NServiceBus persistence packages, and additional guidance for accessing data can be found in the documentation for each persister:

- [SQL](/persistence/sql/accessing-data.md)
- [NHibernate](/persistence/nhibernate/accessing-data.md)
- [MongoDB](/persistence/mongodb/#transactions-shared-transactions)
- [RavenDB](/persistence/ravendb/#shared-session)
- [AWS DynamoDB](/persistence/dynamodb/transactions.md)
- [Service Fabric](/persistence/service-fabric/transaction-sharing.md)
- [CosmosDB](/persistence/cosmosdb/transactions.md#sharing-the-transaction)
- [Azure Table](/persistence/azure-table/transactions.md#sharing-the-transaction)

Synchronized storage session by itself only guarantees that there will be no *partial failures*, i.e., cases where one of the handlers has modified its state while another has not. This guarantee extends to [sagas](/nservicebus/sagas/) as they are persisted using the synchronized storage session.

However, the synchronized storage session **does not guarantee that each state change is persisted exactly once**. To ensure *exactly-once* message processing, the synchronized storage session must support a de-duplication strategy.

### Object/relational mappers

When using an object/relational mapper (ORM) like Entity Framework for data access, as seen in this [sample configuration](/samples/transactional-session/aspnetcore-webapi/#configuration), there is the ability to either inject the data context object via dependency injection or create a data context on the fly and reuse the connection.

Creating a data context on the fly means that any other handler will work disconnected from that data context. This is a fairly simple approach, but it is not recommended when the same message is processed by multiple handlers.

An alternative option that works with multiple handlers processing a single message is to inject the data context of an ORM via dependency injection. More information can be found in the SQL persistence documentation about [accessing data](/persistence/sql/accessing-data.md).

## Message de-duplication strategies

NServiceBus supports multiple message de-duplication strategies that suit a wide range of message processing and data storage technologies.

### Local transactions

The [SQL Server](/transports/sql) and [PostgreSQL](/transports/postgresql) transports allow using a single database transaction to modify application state and send/receive messages. The [persistence](/persistence) detects if the message processing context contains an open transaction and the synchronized storage session joins that transaction. When the transaction is committed, the following are all committed atomically:

- State changes made by handlers
- Receipt of incoming messages
- Sending of outgoing messages

This guarantees that no duplicate messages are sent.

The [SQL Server transport](/transports/sql) shares transaction context with [SQL persistence](/persistence/sql/accessing-data.md) in the `ReceiveOnly`, `SendsAtomicWithReceive`, and `TransactionScope` [transaction modes](/transports/transactions.md) and with [NHibernate persistence](/persistence/nhibernate) in the `TransactionScope` [transaction mode](/transports/transactions.md).

The [PostgreSQL transport](/transports/postgresql) shares transaction context with [SQL persistence](/persistence/sql/accessing-data.md) in the `ReceiveOnly` and `SendsAtomicWithReceive` [transaction modes](/transports/transactions.md).

### Distributed transactions

[Distributed transactions](/transports/transactions.md#transactions-transaction-scope-distributed-transaction) are atomic and durable transactions that span multiple transactional resources (like databases or queues). By enlisting both transport and persistence into the same distributed transaction, NServiceBus can guarantee *exactly-once* message processing by preventing duplicate messages from being created.

Distributed transactions are supported by the following transport and persistence components:

- [SQL persistence](/persistence/sql)
- [NHibernate persistence](/persistence/nhibernate)
- [SQL Server transport](/transports/sql)
- [MSMQ transport](/transports/msmq/) through distributed transactions

To use this mode, the transport must be configured to use the `TransactionScope` [transport transaction mode](/transports/transactions.md). When using the SQL Server transport, the distributed transaction mode allows the use of separate SQL Server instances for message stores (queues) and for business data.

### Outbox

[The outbox](/nservicebus/outbox) is a pattern that provides an *exactly-once* message processing experience even when dealing with transports and databases that don't support distributed transactions, such as RabbitMQ and MongoDB. This is done by storing the incoming message ID and the outgoing messages in the same transaction as the business state change.

The outbox can be used with any transport and with any persistence component that [supports synchronized storage sessions](#synchronized-storage-session).

Instead of preventing the duplicates, the outbox detects them and ensures that the effects of processing duplicate messages are ignored and not persisted.

## Manual de-duplication

In situations where neither of the built-in de-duplication strategies can be applied, the de-duplication of messages must be handled at the application level, in the message handler itself. In these cases, the [synchronized storage session](#synchronized-storage-session) should not be used, and each handler should guarantee the idempotence of its behavior.

### Idempotence caveats

Message-processing logic is idempotent if it can be applied multiple times, and the outcome is the same as if it were applied once. The outcome includes both the application state changes and the potential outgoing messages sent. Consider the following pseudocode that demonstrates **how not to implement idempotent message handling**:

snippet: BusinessData-ManualIdempotence

and think about the behavior of the message processing:

- NServiceBus, by default, defers sending messages until the message handler has finished, so the behavior of the code above is as if the call to `Send` **was after** the call to `ModifyState`.
- If outgoing messages are sent before the state change is committed (e.g., if the code above used [immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately)), there is a risk of creating *ghost messages* -- messages that carry the state change that has never been made durable.
- If outgoing messages are sent after the state change is committed, there is a risk of message loss if the send operation fails. To prevent this, the outgoing messages must be re-sent **even if it appears to be a duplicate**.
- If re-sending messages is implemented, multiple copies of the same message may be sent to the downstream endpoints.
- If message identity is used for de-duplication, message IDs must be generated deterministically.
- If outgoing messages depend on the application state, **the code above is incorrect when messages can get re-ordered** (e.g. by infrastructure failures, [recoverability](/nservicebus/recoverability) or competing consumers).
