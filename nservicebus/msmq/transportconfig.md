---
title: MSMQ transport
summary: 'Explains the mechanics of MSMQ transport, its configuration options and various other configuration settings that were at some point coupled to this transport'
tags:
- Transports
- MSMQ
redirects:
 - nservicebus/msmqtransportconfig
---

Historically, MSMQ is the first transport supported by NServiceBus. In Version 5 it still is by far the most commonly used one. Because of these and also the fact that MSMQ client libraries are included in .NET Base Class Library (`System.Messaging` assembly), MSMQ transport is built into the core of NServiceBus.


## Receiving algorithm

Because of the way MSMQ API has been designed i.e. polling receive that throws an exception when timeout is reached the receive algorithm is more complex than for other polling-driven transports (such as [SQLServer](/nservicebus/sqlserver/)).

The main loop starts by subscribing to `PeekCompleted` event and calling the `BeginPeek` method. When a message arrives the event is raised by the MSMQ client API. The handler for this event starts a new receiving task and waits till this new task has completed its `Receive` call. After that is calls `BeginPeek` again to wait for more messages.


## Configuration

Because of historic reasons, the configuration for MSMQ transport has been coupled to general bus configuration in the previous versions of NServiceBus.


### Queue permissions

By default, queues are created with `Everyone` and `Anonymous Logon` permissions to allow messages to be sent and received without additional configuration. If required, the appropriate permissions can be set on a queue after its creation.

NOTE: From Version 6 if the above default permissions are set, a log message will be written during the transport startup, reminding that the queue is configured with default permissions. During development, if running with an attached debugger, this message will be logged as `INFO` level, otherwise `WARN`.

For more on changing MSMQ permissions, check out the [Administer Security for Message Queuing](https://technet.microsoft.com/en-us/library/cc738047.aspx) section on Microsoft TechNet.


### MSMQ-specific

Following settings are purely related to the MSMQ:

 * `UseDeadLetterQueue`
 * `UseJournalQueue`
 * `UseConnectionCache`
 * `UseTransactionalQueues`

Read [MSMQ connection strings](connection-strings.md) to understand what these settings mean and their defaults.

From Version 4 onwards these settings are configured via a transport connection string (named `nservicebus/transport` for all transports). Before Version 4 some of these properties could be set via `MsmqMessageQueueConfig` configuration section while other (namely the `connectionCache` and the ability to use non-transactional queues) were not available prior to Version 4.

snippet:MessageQueueConfiguration




### MSMQ Label

WARNING: This feature was added in Version 6 and can be used to communicate with Version 5 (and higher) endpoints. However it should **not** be used when communicating to earlier versions (2, 3 or 4) since in those versions the MSMQ Label was used to communicate certain NServiceBus implementation details.

Often when debugging MSMQ using [native tools](viewing-message-content-in-msmq.md) it is helpful to have some custom text in the MSMQ Label. For example the message type or the message id. As of Version 6 the text used to apply to [Message.Label](https://msdn.microsoft.com/library/system.messaging.message.label.aspx) can be controlled at configuration time using the `ApplyLabelToMessages` extension method. This method takes a delegate which will be passed the header collection and should return a string to use for the label. It will be called for all standard messages as well as Audits, Errors and all control messages. The only exception to this rule is received messages with corrupted headers. In some cases it may be useful to use the `Headers.ControlMessageHeader` key to determine if a message is a control message. These messages will be forwarded to the error queue with no label applied. The returned string can be `String.Empty` for no label and must be at most 240 characters.

snippet:ApplyLabelToMessages

## Transactions and delivery guarantees

MSMQ transport supports all [transaction handling modes](/nservicebus/messaging/transactions.md), i.e. Transaction scope, Receive only, Sends atomic with Receive and No transactions.

Refer to [Transport Transactions](/nservicebus/messaging/transactions.md) for detailed explanation of the supported transaction handling modes and available configuration options. 


### Transaction scope

In this mode the ambient transaction is started before receiving the message. The transaction encompasses all stages of processing including user data access and saga data access. If all the logical data stores (transport, user data, saga data) use the same physical store there is no escalation to Distributed Transaction Coordinator (DTC).


### Native transactions

In MSMQ transport there is no distinction between the *ReceiveOnly* and *SendsAtomicWithReceive* levels, they are both handled in an identical way.

The native transaction for receiving messages is shared with sending operations. That means the message receive operation and any send or publish operations are committed atomically.


### Unreliable (Transactions Disabled)

In this mode when message is received from an input queue it's immediately removed from it. If processing fails the message is lost, because the operation cannot be rolled back. Also any other operation performed when processing the message is executed outside of the transaction, it can't be rolled back. That might lead to undesired side effects.
