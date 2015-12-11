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

In this mode the receive transaction is wrapped in a [`TransactionScope`](http://msdn.microsoft.com/en-us/library/system.transactions.transactionscope). All operations inside this scope, both sending messages and manipulating data, are guaranteed to be executed (eventually) as a whole or rolled back as a whole. There is no atomicity guarantee as changes to the database might be visible *before* the messages on the queue or vice-versa. Specific configurations might provide stronger guarantees, but not weaker.

NOTE: This requires the selected storage to support enlisting in distributed transactions.

Depending on transport the transaction is escalated to a distributed one (following two-phase commit protocol) when required. For example is not required when using SQL Server transport with NHibernate persistence both targeted at the same database. Since in this case ADO.NET driver guarantees that everything happens inside a single database transaction, ACID guarantees are held for the whole processing.

NOTE: MSMQ will escalate to a distributed transaction right away since it doesn't support promotable transaction enlistments.

Ambient transaction mode is enabled by default for the transports that support it. It can be enabled explicitly via

snippet:TransactionsEnable

#### Consistency guarantees
In this mode handlers will execute inside of the `TransactionScope` created by the transport. This means that all the data updates are executed as a whole or rolled back as a whole.

Should you not want this behavior just select one of the lower transaction modes supported by the selected transport.

### Transport transaction

In this mode the receive operation is wrapped in a transport native transaction. This mode guarantees that the message is not permanently deleted from the incoming queue until at least one processing attempt (including storing user data and sending out messages) is finished successfully. See the [errors](/nservicebus/errors) section for more details on retries.

Use the following code to request *Transport transaction* mode to be used:

snippet:TransactionsDisableDistributedTransactions

#### Multi queue transactions
Some transports supports enlisting outgoing queue operations in the current receive transaction. This removes the risk of duplicate and ghost messages, see below, being emitted but you still need to make sure that your handlers are idempotent. Currently only the MSMQ and SQL Server transports support multi queue transactions.

#### Consistency guarantees

In this mode some (or all) handlers might get invoked multiple times and partial results might be visible:

 * partial updates - where one handler succeeded updating its data but the other didn't
 * partial sends - where some of the messages has been sent but others not

To handle this you must make sure that you write code that is consistent from a business perspective even though its executed more than once. In other words you must ensure that your handlers are idempotent.

See the `Outbox` section below for details on how NServiceBus can handle [idempotency](/nservicebus/concept-overview#Idempotence) for you at the infrastructure level.

NOTE: Version 5 and below didn't have [batched dispatch](/nservicebus/messaging/batched-dispatch.md) and this meant that messages could be sent out without a matching update to business data depending how you ordered your code. We call this ghost messages. To avoid this you need to make sure that you always perform bus operations as the last statements in your handler taking into account that there can be multiple handlers for a given message.

#### Avoiding partial updates
In this mode there is a risk for partial updates since one handler might succeed in updating business data while other won't. To avoid this you can tell NServiceBus to wrap all handlers in a `TransactionScope` that will act as a unit of work and make sure that there is no partial updates. Use following code to enable a wrapping scope:

snippet:TransactionsWrapHandlersExecutionInATransactionScope

NOTE: This requires storage in use has support for enlisting in transaction scopes

WARNING: This might escalate to a distributed transaction if you're updating data in different databases.

### Unreliable (Transactions Disabled)

In this mode the transport in use does not attempt to wrap the receive operation in any kind of transaction.

Once a message has been received by an Endpoint, if the message processing fails because of an exception within the message handler, it will be put into the error queue. There will be no first level or second level retries. If there is a critical failure, including system or endpoint crash, the message is **permanently lost**. It will not be retried or added to the error queue.

WARNING: In version 5 and below, when transactions are disabled, no retries will be performed and messages will not be forwarded to the error queue in the event of any failure.

snippet:TransactionsDisable

## Outbox

The Outbox [feature](/nservicebus/outbox) provides idempotency on the infrastructure level and allows you to run in *transport transaction* mode while still getting the same semantics as *Ambient transaction* mode.

NOTE: You need to store outbox data in the same database as your business data to achieve the idempotency mentioned above.

When using the outbox, the messages resulting from processing a given received message are not being sent immediately but rather and stored in the persistence database and pushed out after handling logic is done. This mechanism ensures that the handling logic can only succeed once so there is no need to design for idempotence.

## Controlling transaction scope options

NServiceBus allows you to control the following options for transaction scopes created when processing messages.

### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel).

NOTE: Version 3 and below used the default isolation level of .Net which is `Serializable`.

You can change the isolation level using

snippet:CustomTransactionIsolationLevel

### Transaction timeout

NServiceBus will use the [default transaction timeout](https://msdn.microsoft.com/en-us/library/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

You can change the transaction timeout using

snippet:CustomTransactionTimeout

Or via your .config file see an example [here](https://msdn.microsoft.com/en-us/library/system.transactions.configuration.defaultsettingssection%28v=vs.100%29.aspx#Anchor_5).
