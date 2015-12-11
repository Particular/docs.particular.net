---
title: Transactional messaging
summary: Durability guarantees using transactions
tags:
- Transactions
- Durable
- Retry
---

This article covers various levels of consistency guarantees NServiceBus provides with regards to

* receiving messages
* updating user data
* sending messages

It does not discuss the transaction isolation aspect which only applies to the process of updating the user data and does not affect the overall coordination and failure handling.

## Transactions

Based on transaction handling mode, NServiceBus offers three levels of guarantees with regards to message processing. The levels available depends on the capability of the selected transport.

### Ambient transaction (Distributed transaction)

In this mode the receive transaction is wrapped in a [`TransactionScope`](http://msdn.microsoft.com/en-us/library/system.transactions.transactionscope). All operations inside this scope, both sending messages and manipulating data, are guaranteed to be executed (eventually) as a whole or rolled back as a whole.

Depending on transport the transaction is escalated to a distributed one (following two-phase commit protocol) when required. For example is not required when using SQL Server transport with NHibernate persistence both targeted at the same database. Since in this case ADO.NET driver guarantees that everything happens inside a single database transaction, ACID guarantees are held for the whole processing.

NOTE: MSMQ will escalate to a distributed transaction right away since it doesn't support promotable transaction enlistments.

*Ambient transaction* mode is enabled by default for the transports that support it. It can be enabled explicitly via

snippet:TransactionsEnable

#### Consistency guarantees
In this mode handlers will execute inside of the `TransactionScope` created by the transport. This means that all the data updates are executed as a whole or rolled back as a whole.

There is no atomicity guarantee as changes to the database might be visible *before* the messages on the queue or vice-versa. Specific configurations might provide stronger guarantees, but not weaker.

NOTE: This requires the selected storage to support participating in distributed transactions.

### Transport transaction - Receive Only

In this mode the receive operation is wrapped in a transport native transaction. This mode guarantees that the message is not permanently deleted from the incoming queue until at least one processing attempt (including storing user data and sending out messages) is finished successfully. See the [errors](/nservicebus/errors) section for more details on retries.

Use the following code to use this mode:

snippet:TransactionsDisableDistributedTransactions

#### Consistency guarantees

In this mode some (or all) handlers might get invoked multiple times and partial results might be visible:

 * partial updates - where one handler succeeded updating its data but the other didn't
 * partial sends - where some of the messages has been sent but others not

To handle this make sure to write code that is consistent from a business perspective even though its executed more than once. In other words all handlers must be [idempotent](/nservicebus/concept-overview#Idempotence).

See the `Outbox` section below for details on how NServiceBus can handle idempotency at the infrastructure level.

NOTE: Version 5 and below didn't have [batched dispatch](/nservicebus/messaging/batched-dispatch.md) and this meant that messages could be sent out without a matching update to business data depending the order of state,emts. This is called ghost messages. To avoid this make sure to perform all bus operations after any modification to business data. When reviewing the code remember that there can be multiple handlers for a given message. Details on how to enforce message handler ordering can be found [here](/nservicebus/handlers/handler-ordering).

### Transport transaction - Sends atomic with Receive

Some transports supports enlisting outgoing operations in the current receive transaction. This prevents messages being sent to downstream endpoints during retries. Currently only the MSMQ and SQL Server transports support this type of transaction mode.

Use the following code to use this mode:

snippet:TransactionsDisableDistributedTransactions
#### Consistency guarantees
This mode have the same consistency guarantees like the *Receive Only* mode mentioned above with the difference that ghost messages are prevented since all outgoing operations are atomic with the receive operation.

### Unreliable (Transactions Disabled)

In this mode the transport doesn't wrap the receive operation in any kind of transaction. Should the message fail to process it will be moved straight to the error queue. There will be no first level or second level retries since those features relies on transport level transactions.

WARNING: If there is a critical failure, including system or endpoint crash, the message is **permanently lost** since it's received with no transaction.

NOTE: In version 5 and below, when transactions are disabled, no retries will be performed and messages will not be forwarded to the error queue in the event of any failure.

snippet:TransactionsDisable

## Outbox

The Outbox [feature](/nservicebus/outbox) provides idempotency at the infrastructure level and allows running in *transport transaction* mode while still getting the same semantics as *Ambient transaction* mode.

NOTE: Outbox data needs to be stored in the same database as business data to achieve the idempotency mentioned above.

When using the outbox, the messages resulting from processing a given received message are not being sent immediately but rather and stored in the persistence database and pushed out after handling logic is done. This mechanism ensures that the handling logic can only succeed once so there is no need to design for idempotency.

## Avoiding partial updates
In this mode there is a risk for partial updates since one handler might succeed in updating business data while other won't. To avoid this configure NServiceBus to wrap all handlers in a `TransactionScope` that will act as a unit of work and make sure that there is no partial updates. Use following code to enable a wrapping scope:

snippet:TransactionsWrapHandlersExecutionInATransactionScope

NOTE: This requires storage in use has support for enlisting in transaction scopes

WARNING: This might escalate to a distributed transaction if data in different databases are updated.

## Controlling transaction scope options

The following options for transaction scopes used during message processing can be configured.

### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel).

NOTE: Version 3 and below used the default isolation level of .Net which is `Serializable`.

Change the isolation level using

snippet:CustomTransactionIsolationLevel

### Transaction timeout

NServiceBus will use the [default transaction timeout](https://msdn.microsoft.com/en-us/library/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

Change the transaction timeout using

snippet:CustomTransactionTimeout

Or via .config file see an example [here](https://msdn.microsoft.com/en-us/library/system.transactions.configuration.defaultsettingssection%28v=vs.100%29.aspx#Anchor_5).
