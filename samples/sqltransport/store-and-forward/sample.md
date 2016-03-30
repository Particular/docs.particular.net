---
title: SQL Server transport store-and-forward
summary: How to add store-and-forward functionality for external-facing endpoints
reviewed: 2016-03-21
tags:
- SQL Server
- Store-and-forward
related:
- nservicebus/outbox
- nservicebus/sqlserver
redirects:
- samples/sqltransport-outbox-store-and-forward
- samples/outbox/store-and-forward
---


## Prerequisites

 1. Ensure an instance SQL Server Express installed and accessible as `.\SQLEXPRESS`. Create two databases: `sender` and `receiver`.


## Running the project

 1. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 1. Start the Receiver project. 
 1. In the Sender's console is the text `Press <enter> to send a message` when the app is ready.
 1. Hit `<enter>`.


## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.
 1. Hit `<enter>` in the Receiver console to shut it down.
 1. Go to the SQL Server Management Studio and delete the `receiver` database.
 1. Hit `<enter>` again in the Sender console
 1. Notice the retry mechanism kicks in after a 10 seconds delay. It attempts to retry sending the message to the destination database every 10 seconds.
 1. Create the `receiver` database again and restart the Receiver project.
 1. Notice that the message has been delivered to the Receiver.


## Code walk-through

When SQL Server transport is used in the [*multi-instance* mode](/nservicebus/sqlserver/deployment-options.md#modes-overview-multi-instance), the messages are directly inserted into the destination table in another database. If the receiving endpoint's database is down or inaccessible, e.g. because of network failures, the sending endpoint can't send messages to it. In such situation the exception is thrown from the `Send()` or the `Publish()` method, and the message can be lost.

Those problems can be prevented by adding [store-and-forward functionality](/nservicebus/architecture/principles.md#drilling-down-into-details-store-and-forward-messaging) to the SQL Server transport, as explained in this sample. 

NOTE: The [Outbox](/nservicebus/outbox/) would not help solve the issue presented in this example, because it is bypassed when sending messages from outside of a message handler e.g. from MVC controller.

The sample contains three projects:

 * Shared - A class library containing common code including messages definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the order message.

Sender and Receiver use different databases, just like in a production scenario where two systems are integrated using NServiceBus. Each database contains, apart from business data, queues for the NServiceBus endpoint and tables for NServiceBus persistence.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQL Server transport with NHibernate persistence.

snippet:SenderConfiguration

The Sender registers two custom behaviors, one for the send pipeline and the other for the receive pipeline.


#### Send pipeline

The new behavior is added at the beginning of the send pipeline (in Version 5) or in the routing stage (in Version 6).

snippet:SendThroughLocalQueueBehavior

The behavior ignores:
- Messages sent from a handler, because the incoming message will be retried ensuring the outgoing messages are eventually delivered. 
- Deferred messages, because their destination is the local timeout manager satellite. 

The behavior captures the destination of the message in a header and overrides the original value so that the message is actually sent to the local endpoint (put at the end of the endpoint's incoming queue).

NOTICE: In Version 5 other properties of a message (such as defer time) are not captured in this example. In order to use similar feature in production, make sure to capture all relevant information (e.g. defer time).


#### Receive pipeline

In the receive pipeline the new behavior is placed just before loading the message handlers (in Version 5) or in the physical processing stage (in Version 6).

snippet:ForwardBehavior

If the message contains the headers used by the send-side behavior, it is forwarded to the ultimate destination instead of being processed locally. This is the first time the remote database of the Receiver endpoint is contacted. Should it be down, the retry mechanism kicks in and ensures the message is eventually dispatched to the destination. In this example the retry mechanism is configured to retry up to 100 times every 10 seconds.

snippet:SlrConfig

NOTE: In case when sender's and receiver's databases cannot be accessed in a distributed transaction, the `ForwardBehavior` has to include a `TransactionScope` that suppresses the ambient transaction before forwarding the message. 

### Receiver project

The Receiver mimics a back-end system. It is set up to use SQLServer transport with NHibernate persistence. The following code configures it to use the [*multi-instance* mode](/nservicebus/sqlserver/deployment-options.md#modes-overview-multi-instance) of the SQL Server transport.

snippet:ReceiverConfiguration

NOTE: Multi-instance mode is deprecated in Version 6 of NServiceBus (Version 3 of SQL Server transport) and will be removed in the next major version. By that time an alternative store-and-forward solution will be provided.