---
title: MSMQ Transport Configuration
summary: Explains the mechanics of MSMQ transport, its configuration options and various other configuration settings that were at some point coupled to this transport
reviewed: 2016-10-17
component: core
tags:
 - Transport
 - MSMQ
 - Transactions
related:
 - nservicebus/msmq/connection-strings
redirects:
 - nservicebus/msmqtransportconfig
---

MSMQ transport is built into the core NServiceBus NuGet package.


## Receiving algorithm

Because of the way MSMQ API has been designed i.e. polling receive that throws an exception when timeout is reached the receive algorithm is more complex than for other polling-driven transports (such as [SQLServer](/nservicebus/sqlserver/)).

The main loop starts by subscribing to `PeekCompleted` event and calling the `BeginPeek` method. When a message arrives the event is raised by the MSMQ client API. The handler for this event starts a new receiving task and waits till this new task has completed its `Receive` call. After that is calls `BeginPeek` again to wait for more messages.


## Queue permissions

By default, queues are created with `Everyone` and `Anonymous Logon` permissions to allow messages to be sent and received without additional configuration. If required, the appropriate permissions can be set on a queue after its creation.

NOTE: From Version 6 if the above default permissions are set, a log message will be written during the transport startup, reminding that the queue is configured with default permissions. During development, if running with an attached debugger, this message will be logged as `INFO` level, otherwise `WARN`.

See also [Message Queuing Security Overview in Windows Server 2008](https://technet.microsoft.com/en-us/library/cc771268.aspx).


partial: label


## Transactions and delivery guarantees

MSMQ Transport supports the following [Transport Transaction Modes](/nservicebus/transports/transactions.md):

 * Transaction scope (Distributed transaction)
 * Transport transaction - Sends atomic with Receive
 * Transport transaction - Receive Only
 * Unreliable (Transactions Disabled)

See also [Controlling Transaction Scope Options](/nservicebus/transports/transactions.md#controlling-transaction-scope-options).


### Transaction scope (Distributed transaction)

In this mode the ambient transaction is started before receiving the message. The transaction encompasses all stages of processing including user data access and saga data access. If all the logical data stores (transport, user data, saga data) use the same physical store there is no escalation to Distributed Transaction Coordinator (DTC).


partial: native-transactions


### Unreliable (Transactions Disabled)

In this mode, when a message is received, it is immediately removed from the input queue. If processing fails the message is lost because the operation cannot be rolled back. Any other operation that is performed, when processing the message, is executed without a transaction and cannot be rolled back. This can lead to undesired side effects when message processing fails part way through.
