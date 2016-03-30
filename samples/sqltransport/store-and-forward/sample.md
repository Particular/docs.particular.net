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

 1. Ensure an instance SQL Server Express installed and accessible as `.\SQLEXPRESS`. Create two databases: `sender` and `receiver`.
 1. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 1. Start the Receiver project. 
 1. In the Sender's console is the text `Press <enter> to send a message` when the app is ready.
 1. Hit <enter>.
 1. On the Receiver console the order was submitted.
 1. On the Sender console the order was accepted.
 1. Hit <enter> in the Receiver console to shut it down.
 1. Go to the SQL Server Management Studio and delete the `receiver` database.
 1. Hit <enter> again in the Sender console
 1. Notice the retry mechanism kicking in after a 10 second delay. It attempts to retry sending the message to the destination database every 10 seconds.
 1. Create the `receiver` database again and restart the Receiver project.
 1. Notice that the message has been delivered to the Receiver.


## Code walk-through

This sample shows how to add store-and-forward functionality to SQL Server transport. In such scenario if the receiver (back-end) endpoint's database is down (e.g. the device running the sender endpoint disconnected from the network where receiver's database is located), the sender (front-end, user facing) endpoint can't send messages to it. This happens because in the multi-instance mode the messages are directly inserted into the destination table in the destination database. The exception is thrown from the `Send`/`Publish` method which inevitably results in a bad user experience (UX).

In order to provide a better UX, a store-and-forward functionality using local sender's database is required.

NOTE: The [Outbox](/nservicebus/outbox/index.md) would not help solve this issue because it is bypassed when sending messages from outside of a message handler e.g. from MVC controller.

This sample contains three projects:

 * Shared - A class library containing common code including the message definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the order message.

Sender and Receiver use different databases, just like in a production scenario where two systems are integrated using NServiceBus. Each database contains, apart from business data, queues for the NServiceBus endpoint and tables for NServiceBus persistence.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQL Server transport with NHibernate persistence.

snippet:SenderConfiguration

It also registers two custom behaviors, one for the send pipeline and the other for the receive pipeline.


### Send pipeline

The new behavior is added at the beginning of the send pipeline (in Version 5) or in the routing stage (in Version 6).

snippet:SendThroughLocalQueueBehavior

The behavior ignores messages coming from a handler or deferred. In the first case the incoming message will be retried ensuring the outgoing messages are eventually delivered. In the second the destination is the local timeout manager satellite. 

It captures the destination of the message in a header and overrides the original value so that the message is actually send to the local endpoint (put in the end of the endpoint's incoming queue).

NOTICE: In Version 5 other properties of a message (such as defer time) are not captured in this example. In order to use similar feature in production, make sure to capture all relevant information (e.g. defer time).


### Receive pipeline

In the receive pipeline the new behavior is placed just before loading the message handlers (in Version 5) or in the physical processing stage (in Version 6).

snippet:ForwardBehavior

If the message contains the headers used by the send-side behavior, it is forwarded to the ultimate destination instead of being processed locally. This is the first time the remote database of Receiver endpoint is contacted. Should it be down, the retry mechanism kicks in and ensures the message is eventually dispatched to the destination. In this example the retry mechanism is configured to retry up to 100 times every 10 seconds

snippet:SlrConfig

### Receiver project

The Receiver mimics a back-end system. It is set up to use SQLServer transport with NHibernate persistence. Following code configures it to use the multi-instance (formerly multi-database) mode of SQL Server transport.

snippet:ReceiverConfiguration

NOTE: Multi-instance mode is deprecated in Version 6 of NServiceBus (Version 3 of SQL Server transport) and will be removed in the next major version. By that time an alternative store-and-forward solution will be provided.