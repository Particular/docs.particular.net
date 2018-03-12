---
title: Store-and-forward
summary: Add store-and-forward functionality for external-facing endpoints.
reviewed: 2018-03-08
component: SqlTransport
related:
- nservicebus/outbox
- transports/sql
redirects:
- samples/sqltransport-outbox-store-and-forward
- samples/outbox/store-and-forward
---

NOTE: This way of implementing store-and-forward behavior is no longer available in Versions 4 and later of the SQL Server transport. In order to achieve similar behavior in Version 4 and later, use [Switch](/samples/bridge/sql-switch) or [Backplane](/samples/bridge/backplane).

## Prerequisites

include: sql-prereq

The databases created by this sample are `NsbSamplesStoreAndForwardReceiver` and `NsbSamplesStoreAndForwardSender`.


## Running the project

 1. Start the Sender project (right-click the project, select `Debug > Start new instance`).
 1. Start the Receiver project.
 1. In the Sender's console, `Press <enter> to send a message` appears when the app is ready.
 1. Press enter.


## Verifying the sample works correctly

 1. The Receiver indicates that the order was submitted.
 1. The Sender indicates that the order was accepted.
 1. Press enter in the Receiver console to shut it down.
 1. Go to SQL Server Management Studio and delete the `NsbSamplesStoreAndForwardReceiver` database.
 1. Press enter again in the Sender console
 1. Notice that the retry mechanism kicks in after 10 seconds and retries sending the message to the destination database.
 1. Restart the Receiver project.
 1. Notice that the message has been delivered to the Receiver.


## Code walk-through

When the SQL Server transport is used in [*multi-instance* mode](/transports/sql/deployment-options.md?version=SqlTransport_3#multi-instance.md), the messages are inserted directly into the remote destination database's table. If the receiving endpoint's database is down or inaccessible (for example, because of network failures), the sending endpoint can't send messages to it. In this situation, the exception is thrown from the `Send()` or the `Publish()` methods, resulting in a potential message loss.

The message loss problem can be prevented by adding [store-and-forward functionality](/nservicebus/architecture/principles.md#messaging-versus-rpc-store-and-forward-messaging) to the SQL Server transport, as explained in this sample.

NOTE: The [Outbox](/nservicebus/outbox/) would not solve the issue presented in this example because it is bypassed when sending messages from outside a message handler, e.g. from an ASP.NET MVC controller.

The sample contains three projects:

 * Shared - A class library containing common code including messages definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the `OrderSubmitted` message.

Sender and Receiver use different databases, just like in a production scenario where two systems are integrated using NServiceBus. Each database contains, apart from business data, queues for the NServiceBus endpoint.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use the  SQL Server transport.

snippet: SenderConfiguration

The Sender registers two custom behaviors, one for the send pipeline and one for the receive pipeline.


#### Send pipeline

The new behavior is added at the beginning of the send pipeline (in version 2) or in the routing stage (in version 3).

snippet: SendThroughLocalQueueBehavior

The behavior ignores:

 * Messages sent from a handler, because the incoming message will be retried ensuring the outgoing messages are eventually delivered.
 * Deferred messages, because their destination is the local timeout manager satellite.

The behavior captures the destination of the message in a header and overrides the original value so that the message is actually sent to the local endpoint (at the end of the endpoint's incoming queue).

NOTE: In version 3 of this sample, some properties of a message (such as defer time) are not handled. In order to use similar feature in production, add code to handle all possible situations or refrain from using deferred messages in endpoints where store-and-forward is used.


#### Receive pipeline

In the receive pipeline, the new behavior is placed just before loading the message handlers (in version 2) or in the physical processing stage (in version 3).

snippet: ForwardBehavior

If the message contains the headers used by the sender-side behavior, it is forwarded to the ultimate destination instead of being processed locally. This is the first time the remote database of the receiver endpoint is contacted. Should it be down, the retry mechanism kicks in and ensures the message is eventually delivered to the destination. In this example, the retry mechanism is configured to retry every 10 seconds up to 100 times.

snippet: DelayedRetriesConfig

NOTE: When both the sender's and receiver's databases cannot be accessed in a distributed transaction, the `ForwardBehavior` has to include a `TransactionScope` that suppresses the ambient transaction before forwarding the message.


### Receiver project

The Receiver mimics a back-end system. The following code configures it to use [*multi-instance* mode](/transports/sql/deployment-options.md?version=SqlTransport_3#multi-instance.md) of the SQL Server transport.

snippet: ReceiverConfiguration

NOTE: Multi-instance mode is deprecated in version 3 of the SQL Server transport and will be removed in the next major version. An alternative store-and-forward solution will be provided. For more information refer to the [SQL Server transport version 2 to version 3 upgrade guide](/transports/upgrades/sqlserver-2to3.md#namespace-changes-multi-instance-support).
