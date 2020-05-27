---
title: Accessing and modifying data from a message handler
summary: How to access business data from a message handler in sync with message consumption and modifications to NServiceBus-controlled data.
component: Core
versions: '[6,)'
reviewed: 2020-04-28
related:
 - persistence/nhibernate/accessing-data
 - persistence/sql/accessing-data
 - persistence/mongodb
 - persistence/ravendb
---

In most cases [handlers](/nservicebus/handlers/) are meant to modify the internal state of an application based on the received message. In a messaging system it is critical to make sure the state change is persisted exactly once. The scenarios below discuss in detail how NServiceBus transaction and persistence settings affect the way business data is stored.


## Synchronized storage session

Synchronized storage session is NServiceBus's built-in implementation of Unit of Work pattern. It provides a data access context that is shared by all handlers that process a given message. The state change is committed after the execution of the last handler, provided that there were no exceptions during processing. The synchronized storage session is accessible via `IMessageHandlerContext`:

snippet: BusinessData-SynchronizedStorageSession

The synchronized storage session feature is supported by most NServiceBus persistence packages:
 - [SQL](/persistence/sql/accessing-data.md)
 - [NHibernate](/persistence/nhibernate/accessing-data.md)
 - [MongoDB](/persistence/mongodb/#transactions-shared-transactions)
 - [RavenDB](/persistence/ravendb/#shared-session)

Synchronized storage session by itself only guarantees that there will be no *partial failures* i.e. cases where one of the handlers has modified its state while another has not. This guarantee extends to [sagas](/nservicebus/sagas/) as they are persisted using the synchronized storage session.

However, the synchronized storage session **does not guarantee that each state change is persisted exactly once**. To ensure *exactly-once* message processing, synchronized storage session has to support a de-duplication strategy.


## Message de-duplication strategies

NServiceBus supports multiple message de-duplication strategies that suit a wide range of message processing and data storage technologies. 


### Local transactions

[SQL Server transport](/transports/sql) is unique among NServiceBus transports as it allows using a single SQL Server transaction to modify the application state and send/receive messages. Both NHibernate and SQL persistence automatically detect if the message processing context contains an open transaction. If this transaction points to a database that the persister is configured to use, the synchronized storage session wraps that transport transaction. As a result, the state changes requested by the handlers are committed atomically when consuming the incoming message and sending all outgoing messages. This guarantees that no duplicate messages are created in the system.

[SQL persistence](/persistence/sql/accessing-data.md) can be used in the shared local transaction mode in `SendsAtomicWithReceive` and `TransactionScope` [transaction modes](/transports/transactions.md). [NHibernate persistence](/persistence/nhibernate) can only shared the transaction context with the transport if configured to use `TransactionScope` transaction mode. In both cases, however, the transaction **will not be escalated** to a distributed transaction.


### Distributed transactions

[Distributed transactions](/transports/transactions.md#transactions-transaction-scope-distributed-transaction) are atomic and durable transactions that span multiple transactional resources (like databases or queues). By enlisting both transport and persistence into the same distributed transaction NServiceBus can guarantee *exactly-once* message processing by preventing duplicate messages from being created.

Distributed transactions are supported by the following transport and persistence components:
 - [SQL persistence](/persistence/sql)
 - [NHibernate persistence](/persistence/nhibernate)
 - [SQL Server transport](/transports/sql)
 - [MSMQ transport](/transports/msmq/) through distributed transactions

In order to use this mode, the transport must be configured to use `TransactionScope` [mode](/transports/transactions.md). When using the SQL Server transport, the distributed transaction mode allows using separate SQL Server instances for message stores (queues) and for business data.


### Outbox

[The outbox](/nservicebus/outbox) is a pattern that provides *exactly-once* message processing experience even when dealing with transports and databases that don't support distributed transactions, such as RabbitMQ and MongoDB. This is done by storing the incoming message ID and the outgoing messages in the same transaction as the business state change.

The outbox can be used with any transport and with any persistence component that [supports synchronized storage sessions](#synchronized-storage-session).

Instead of preventing the duplicates, the outbox detects them and ensures that the effects of processing duplicate messages are ignored and not persisted.


## Manual de-duplication

In situations where neither of the built-in de-duplication strategies can be applied, the de-duplication of messages must be handled at the application level, in the message handler itself. In these cases the [synchronized storage session](#synchronized-storage-session) should not be used and each handler should guarantee the idempotence of its behavior.

### Idempotence caveats

Message-processing logic is idempotent if it can be applied multiple times and the outcome is the same as if it were applied once. The outcome includes both the application state changes and the potential outgoing messages sent. Consider the following pseudocode that demonstrates **how not to implement idempotent message handling**:

snippet: BusinessData-ManualIdempotence

and think about the behavior of the message processing:

 - NServiceBus by default defers sending messages until the message handler has finished so the behavior of the code above is as if the call to `Send` **was after** the call to `ModifyState`
 - If outgoing messages are sent before the state change is committed (e.g if the code above used [immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately)) there is a risk of creating *ghost messages* -- messages that carry the state change that has never been made durable
 - If outgoing messages are sent after the state change is committed there is risk of message loss if the send operation fails. To prevent this, the outgoing messages must be re-sent **even if it appears to be a duplicate**
 - If re-sending messages is implemented, multiple copies of the same message may be sent to the downstream endpoints  
 - If message identity is used for de-duplication, message IDs must be generated in a deterministic manner
 - If outgoing messages depend on the application state, **the code above is incorrect when messages can get re-ordered** (e.g. by infrastructure failures, [recoverability](/nservicebus/recoverability) or competing consumers)
