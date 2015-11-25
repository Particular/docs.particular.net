---
title: Transactional messaging
summary: Durability guarantees using transactions
tags:
- Transactions
- Durable
- Retry
---

This article covers various levels of guarantees NServiceBus provides with regards to

* coordination of receiving messages
* updating user data 
* sending out other messages

It does not discuss the transaction isolation aspect which only applies to the process of updating the user data and does not affect the overall coordination and failure handling.


## Transactions

Based on transaction handling mode, NServiceBus offers three levels of guarantees with regards to message processing


### Unreliable (Transactions Disabled)

In this mode the transport in use does not attempt to wrap the receive operation in any kind of transaction. 

Once a message has been received by an Endpoint, if the message processing fails because of an exception within the message handler, it will be put into the error queue. There will be no first level or second level retries. If there is a critical failure, including system or endpoint crash, the message is **permanently lost**. It will not be retried or added to the error queue.

WARNING: In version 5 and below, when transactions are disabled, no retries will be performed and messages will not be forwarded to the error queue in the event of any failure.

snippet:TransactionsDisable


### Transport transaction

In this mode, if supported by the transport, the receive operation is wrapped in a native transaction. The same transaction is used when processing of the message results in sending out other messages. This mode guarantees that the message is not permanently deleted from the incoming queue until at least one processing attempt (including storing user data and sending out messages) is finished successfully.

NOTE: In this mode messages on the wire can get duplicated at each endpoint so the handler logic has to be designed to be idempotent.

snippet:TransactionsDisableDistributedTransactions


### Ambient transaction

In this mode, if supported by the transport, the receive transaction is wrapped in `TransactionScope`. All the operations inside this scope, both sending messages and manipulating data, are guaranteed to be executed (eventually) as a whole or rolled back as a whole. There is no atomicity guarantee as changes to the database might be visible *before* the messages on the queue or vice-versa. Specific configurations might provide stronger guarantees, but not weaker.

That **does not** mean that a distributed transaction is started right away. The transaction is only escalated to the distributed one (following two-phase commit protocol) when it is required. For example is not required when using SQL Server transport with NHibernate persistence and both in the same database. Since in this case ADO.NET driver guarantees that everything happens inside a single database transaction, ACID guarantees are held for the whole processing.

Ambient transaction mode is enabled by default. It can be enabled explicitly via

snippet:TransactionsEnable

#### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel). 

NOTE: Version 3 and below used the default isolation level of .Net which is `Serializable`.

You can change the isolation level using

snippet:CustomTransactionIsolationLevel

#### Transaction timeout

NServiceBus will use the [default transaction timeout](https://msdn.microsoft.com/en-us/library/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

You can change the transaction timeout using

snippet:CustomTransactionTimeout

Or via your *.config file see an example [here](https://msdn.microsoft.com/en-us/library/system.transactions.configuration.defaultsettingssection%28v=vs.100%29.aspx#Anchor_5). 


## Handlers

In the ambient transaction mode NServiceBus executes all message handlers in a single `TransactionScope` in order to ensure that all the data manipulations are either executed as a whole or rolled back as a whole. This behavior can be disabled using the following

<!-- import TransactionsDoNotWrapHandlersExecutionInATransactionScope --> 

This results in suppressing any ambient transaction that exist. This effectively turns the *ambient transaction* mode into *transport transaction* mode with regards to handler behavior (the only difference is the type of transaction used by the transport). Some (or all) handlers might get invoked multiple times and partial results might be visible: 

 * partial updates (where one handler succeeded updating its data but the other didn't), 
 * partial sends (where some of the messages has been sent but others not),
 * sends without matching updates (where messages has already been sent but the update failed).


NOTE: Starting with version 5, in the *transport transaction* and *unreliable* modes this behavior is the default. Wrapping the message handlers with `TransactionScope` has to be enabled explicitly.

snippet:TransactionsWrapHandlersExecutionInATransactionScope

## Outbox

Outbox is a [feature](/nservicebus/outbox)  that enhances the *transport transaction* mode guarantees. 

snippet:TransactionsOutbox

Enabled by default only for RabbitMQ transport, requires explicit enabling when using other transports.

When using the outbox, the messages resulting from processing a given received message are not being sent immediately but rather and stored in the persistence database and pushed out after handling logic is done. This mechanism ensures that the handling logic can only succeed once so there is no need to design for idempotence.
