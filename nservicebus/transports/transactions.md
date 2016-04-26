---
title: Transports Transactions
summary: Supported transaction modes and their consistency guarantees
tags:
- Transactions
- Retries
- Consistency
- Transports
- TransactionScope
redirects:
- nservicebus/messaging/transactions
reviewed: 2016-04-21
---

This article covers various levels of consistency guarantees NServiceBus provides with regards to:

 * receiving messages
 * updating user data
 * sending messages

It does not discuss the transaction isolation aspect. Transaction isolation applies only to the process of updating the user data, it does not affect the overall coordination and failure handling.


## Transactions

NServiceBus offers four levels of guarantees with regards to message processing. Levels availability depends on the selected transport. See also [Transaction handling in Azure](/nservicebus/azure/transactions.md).


### Transaction levels supported by NServiceBus transports


#### Versions 6 and above

The implementation details for each transport are discussed in the dedicated documentation sections. They can be accessed by clicking the links with the transport name in the following table:

|  | [Transaction scope (Distributed transaction)](/nservicebus/transports/transactions.md#transactions-transaction-scope-distributed-transaction) | [Transport transaction - Sends atomic with Receive](/nservicebus/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive)  | [Transport transaction - Receive Only](/nservicebus/transports/transactions.md#transactions-transport-transaction-receive-only) | [Unreliable (Transactions Disabled)](/nservicebus/transports/transactions.md#transactions-unreliable-transactions-disabled) |
| :------------------| :-: |:-:| :-:| :-: |
| [MSMQ](/nservicebus/msmq/transportconfig.md#transactions-and-delivery-guarantees) | &#10004; | &#10004; | &#10004; | &#10004; |
| [SQL Server](/nservicebus/sqlserver/design.md#transactions-and-delivery-guarantees) | &#10004; | &#10004; | &#10004; | &#10004; |
| [RabbitMQ](/nservicebus/rabbitmq/configuration-api.md#transactions-and-delivery-guarantees) | &#10006; | &#10006; | &#10004; | &#10004; |
| [Azure Storage Queues](/nservicebus/azure-storage-queues/transaction-support.md#transactions-and-delivery-guarantees)| &#10006; | &#10006; | &#10004; | &#10004; |
| [Azure Service Bus](/nservicebus/azure-service-bus/transaction-support.md#transactions-and-delivery-guarantees) | &#10006; | &#10004; | &#10004; | &#10004; |


### Transaction scope (Distributed transaction)

In this mode the transport receive operation is wrapped in a [`TransactionScope`](https://msdn.microsoft.com/en-us/library/system.transactions.transactionscope). Other operations inside this scope, both sending messages and manipulating data, are guaranteed to be executed (eventually) as a whole or rolled back as a whole.

If required the transaction is escalated to a distributed one (following two-phase commit protocol). That depends on the transport and persistence choice, for example it is not needed when using [SQL Server transport](/nservicebus/sqlserver/) with [NHibernate persistence](/nservicebus/nhibernate/) both using the same database. In this case the ADO.NET driver guarantees that everything happens inside a single database transaction, ACID guarantees are held for the whole processing.

NOTE: MSMQ will escalate to a distributed transaction right away since it doesn't support promotable transaction enlistments.

*Transaction scope* mode is enabled by default for the transports that support it (i.e. MSMQ and SQL Server transport). It can be enabled explicitly via

snippet:TransportTransactionScope


#### Consistency guarantees

In this mode handlers will execute inside the `TransactionScope` created by the transport. This means that all the data updates are executed as a whole or rolled back as a whole.

Distributed transactions do not guarantee atomicity. Changes to the database might be visible *before* the messages disappear from the queue or vice-versa, but they are guaranteed to eventually all sync up to reflect the outcome of the transaction.

NOTE: This mode requires the selected storage to support participating in distributed transactions.


### Transport transaction - Sends atomic with Receive

Some transports support enlisting outgoing operations in the current receive transaction. This prevents messages being sent to downstream endpoints during retries.

Use the following code to use this mode:

snippet:TransportTransactionAtomicSendsWithReceive


#### Consistency guarantees

This mode has the same consistency guarantees as the *Receive Only* mode, but additionally it prevents occurrence of *ghost messages* since all outgoing operations are atomic with the ongoing receive operation.


### Transport transaction - Receive Only

In this mode the receive operation is wrapped in a transport's native transaction. This mode guarantees that the message is not permanently deleted from the incoming queue until at least one processing attempt (including storing user data and sending out messages) is finished successfully. See also [errors](/nservicebus/errors) for more details on retries.

Use the following code to use this mode:

snippet:TransportTransactionReceiveOnly

NOTE: Prior to Version 6 receive only mode couldn't be requested for transports supporting the `atomic sends with receive` mode (see below).


#### Consistency guarantees

In this mode some (or all) handlers might get invoked multiple times and partial results might be visible:

 * partial updates - where one handler succeeded updating its data but the other didn't
 * partial sends - where some of the messages has been sent but others not

When using this mode all handlers must be [idempotent](/nservicebus/concept-overview.md#idempotence). In other words the result needs to be consistent from a business perspective even when the message is processed more than once.

See the `Outbox` section below for details on how NServiceBus can handle idempotency at the infrastructure level.

NOTE: Versions 5 and below do not support [batched dispatch](/nservicebus/messaging/batched-dispatch.md) and this meant that messages could be sent out without a matching update to business data, depending on the order in which  statements were executed. Such messages are called *ghost messages*. To avoid this situation make sure to perform all bus operations only after modifications to business data. When reviewing the code remember that there can be multiple handlers for a given message and that [handlers are executed in a certain order](/nservicebus/handlers/handler-ordering.md).


### Unreliable (Transactions Disabled)

Disabling transactions is generally not recommended, because it might lead to the message loss. It might be considered if losing some messages is not problematic and if the messages get outdated quickly, e.g. when sending readings from sensors at regular intervals.

DANGER: In this mode, when encountering a critical failure such as system or endpoint crash, the message is **permanently lost**.

snippet:TransactionsDisable


#### Versions 6 and above

In this mode the transport doesn't wrap the receive operation in any kind of transaction. Should the message fail to process it will be moved straight to the error queue.


#### Versions 5 and below

In Versions 5 and below, when transactions are disabled, no retries will be performed and messages **will not be forwarded** to the error queue in the event of any failure and the message will be permanently lost.


## Outbox

The [Outbox](/nservicebus/outbox) feature provides idempotency at the infrastructure level and allows running in *transport transaction* mode while still getting the same semantics as *Transaction scope* mode.

NOTE: Outbox data needs to be stored in the same database as business data to achieve the `exactly-once delivery`.

When using the Outbox, any messages resulting from processing a given received message are not sent immediately but rather stored in the persistence database and pushed out after the handling logic is done. This mechanism ensures that the handling logic can only succeed once so there is no need to design for idempotency.


## Avoiding partial updates

In this mode there is a risk of partial updates since one handler might succeed in updating business data while another handler fails. To avoid this configure NServiceBus to wrap all handlers in a `TransactionScope` that will act as a unit of work and make sure that there is no partial updates. Use following code to enable a wrapping scope:

snippet:TransactionsWrapHandlersExecutionInATransactionScope

NOTE: This requires the selected storage to support enlisting in transaction scopes.

WARNING: This might escalate to a distributed transaction if data in different databases are updated.

WARNING: This API must not be used in combination with transports running in a *transaction scope* mode. Starting from version 6, wrapping handlers in a `TransactionScope` in such a situation throws an exception.


## Controlling transaction scope options

The following options for transaction scopes used during message processing can be configured.

NOTE: In Versions 6 and above isolation level and timeout for transaction scopes are also configured at the transport level.


### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel).

NOTE: Versions 3 and below used the default isolation level of .Net which is `Serializable`.

Change the isolation level using

snippet:CustomTransactionIsolationLevel


### Transaction timeout

NServiceBus will use the [default transaction timeout](https://msdn.microsoft.com/en-us/library/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

Change the transaction timeout using

snippet:CustomTransactionTimeout

Or via .config file using a [example DefaultSettingsSection](https://msdn.microsoft.com/en-us/library/system.transactions.configuration.defaultsettingssection.aspx#Anchor_5).
