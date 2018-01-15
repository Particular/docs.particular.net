---
title: Store-and-forward
summary: Add store-and-forward functionality for external-facing endpoints.
reviewed: 2016-04-27
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

 1. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 1. Start the Receiver project.
 1. In the Sender's console is the text `Press <enter> to send a message` when the app is ready.
 1. Hit enter.


## Verifying that the sample works correctly

 1. The Receiver displays information that the order was submitted.
 1. The Sender displays information that the order was accepted.
 1. Hit enter in the Receiver console to shut it down.
 1. Go to the SQL Server Management Studio and delete the `NsbSamplesStoreAndForwardReceiver` database.
 1. Hit enter again in the Sender console
 1. Notice the retry mechanism kicks in after a 10 seconds and retries sending the message to the destination database.
 1. Restart the Receiver project.
 1. Notice that the message has been delivered to the Receiver.


## Code walk-through

When SQL Server transport is used in the [*multi-instance* mode](/transports/sql/deployment-options.md?version=SqlTransport_3#multi-instance.md), the messages are inserted directly into the remote destination database's table. If the receiving endpoint's database is down or inaccessible, e.g. because of network failures, the sending endpoint can't send messages to it. In such situations the exception is thrown from the `Send()` or the `Publish()` methods, resulting in a potential message loss.

The message loss problem can be prevented by adding [store-and-forward functionality](/nservicebus/architecture/principles.md#messaging-versus-rpc-store-and-forward-messaging) to the SQL Server transport, as explained in this sample.

NOTE: The [Outbox](/nservicebus/outbox/) would not help solve the issue presented in this example, because it is bypassed when sending messages from outside of a message handler, e.g. from ASP.NET MVC controller.

The sample contains three projects:

 * Shared - A class library containing common code including messages definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the `OrderSubmitted` message.

Sender and Receiver use different databases, just like in a production scenario where two systems are integrated using NServiceBus. Each database contains, apart from business data, queues for the NServiceBus endpoint.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQL Server transport.

snippet: SenderConfiguration

The Sender registers two custom behaviors, one for the send pipeline and one for the receive pipeline.


#### Send pipeline

The new behavior is added at the beginning of the send pipeline (in Version 2) or in the routing stage (in Version 3).

snippet: SendThroughLocalQueueBehavior

The behavior ignores:

 * Messages sent from a handler, because the incoming message will be retried ensuring the outgoing messages are eventually delivered.
 * Deferred messages, because their destination is the local timeout manager satellite.

The behavior captures the destination of the message in a header and overrides the original value so that the message is actually sent to the local endpoint (put at the end of the endpoint's incoming queue).

NOTICE: In Version 3 of this sample some properties of a message (such as defer time) are not handled. In order to use similar feature in production, make sure to add code to handle all possible situations or refrain from using deferred messages in endpoints where store-and-forward is used.


#### Receive pipeline

In the receive pipeline the new behavior is placed just before loading the message handlers (in Version 2) or in the physical processing stage (in Version 3).

snippet: ForwardBehavior

If the message contains the headers used by the send-side behavior, it is forwarded to the ultimate destination instead of being processed locally. This is the first time the remote database of the Receiver endpoint is contacted. Should it be down, the retry mechanism kicks in and ensures the message is eventually delivered to the destination. In this example the retry mechanism is configured to retry every 10 seconds for up to 100 times.

snippet: DelayedRetriesConfig

NOTE: When both sender's and receiver's databases cannot be accessed in a distributed transaction, the `ForwardBehavior` has to include a `TransactionScope` that suppresses the ambient transaction before forwarding the message.


### Receiver project

The Receiver mimics a back-end system. The following code configures it to use the [*multi-instance* mode](/transports/sql/deployment-options.md?version=SqlTransport_3#multi-instance.md) of the SQL Server transport.

snippet: ReceiverConfiguration

NOTE: Multi-instance mode is deprecated in Version 3 of SQL Server transport and will be removed in the next major version. By that time an alternative store-and-forward solution will be provided. For more information refer to the [SQL Server transport Version 2 to Version 3 upgrade guide](/transports/upgrades/sqlserver-2to3.md#namespace-changes-multi-instance-support).
