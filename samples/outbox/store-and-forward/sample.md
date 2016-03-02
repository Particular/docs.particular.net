---
title: Outbox Store-and-forward
summary: How to add store-and-forward functionality for external-facing endpoints
tags:
- SQL Server
- Outbox
- Store-and-forward
related:
- nservicebus/outbox
- nservicebus/sqlserver
redirects:
- samples/sqltransport-outbox-store-and-forward
---

 1. Make sure you have SQL Server Express installed and accessible as `.\SQLEXPRESS`. Create three databases: `sender`, `receiver` and `shared`.
 2. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 3. Start the Receiver project.
 4. If you see `DtcRunningWarning` log message in the console, it means you have a Distributed Transaction Coordinator (DTC) service running. The Outbox feature is designed to provide *exactly once* delivery guarantees without DTC. It is better to disable the DTC service to avoid confusion when using Outbox.
 5. In the Sender's console you should see `Press <enter> to send a message` text when the app is ready.
 6. Hit <enter>.
 7. On the Receiver console you should see that order was submitted.
 8. On the Sender console you should see that the order was accepted.
 9. Hit <enter> in the Receiver console to shut it down.
 10. Go to the SQL Server Management Studio and delete the `receiver` database.
 11. Hit <enter> again in the Sender console
 12. You should see the retry mechanism kicking in, doing some number of first-level retries and then forwarding the message to the SLR.


## Code walk-through

This sample show how to add store-and-forward functionality to any a transport that does not have it. In this case the SQL Server transport is used with each endpoint using its own database (server) but the solution is not dependent on the SQL Server transport in any way.

In such scenario if the receiver (back-end) endpoint's database is down (e.g. for maintenance), the sender (front-end, user facing) endpoint can't send messages to it. This happens because even when the Outbox is enabled, the messages that are send from outside of a handler bypass the Outbox and are immediately dispatched to the transport (which in this case means inserting into the destination table in the destination database). The exception is thrown from the `Send`/`Publish` method which inevitably results in a bad user experience (UX).

In order to provide a better UX, a store-and-forward functionality using local sender's database is required.

This sample contains three projects:

 * Shared - A class library containing common code including the message definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the order message.

Sender and Receiver use different databases, just like in a production scenario where two systems are integrated using NServiceBus. Each database contains, apart from business data, queues for the NServiceBus endpoint and tables for NServiceBus persistence.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQL Server transport with NHibernate persistence and Outbox.

snippet:SenderConfiguration

It also registers two custom behaviors, one for the send pipeline and the other for the receive pipeline.


### Send pipeline

The new behavior is added at the beginning of the send pipeline.

snippet:OutboxLoopbackSendBehavior

It checks if the message comes from a handler and in such case it does nothing. Otherwise it captures the destination of the message in a header and overrides the original value so that the message is actually send to the local endpoint (put in the end of the endpoint's incoming queue).

NOTICE: Other properties of a message (such as defer time) are not captured in this example. In order to use similar feature in production, make sure to capture all relevant information (e.g. defer time).


### Receive pipeline

In the receive pipeline the new behavior is placed just before loading the message handlers.

snippet:OutboxLoopbackReceiveBehavior

If the message contains the headers used by the send-side behavior, it is routed to the ultimate destination (this time via the Outbox) instead of being processed locally. This is the first time the remote database of Receiver endpoint is contacted. Should it be down, the retry mechanism will kick in and ensure the message is eventually dispatched to the destination.


### Receiver project

The Receiver mimics a back-end system. It is also configured to use SQLServer transport with NHibernate persistence and Outbox but uses V2.1 code-based connection information API.

snippet:ReceiverConfiguration
