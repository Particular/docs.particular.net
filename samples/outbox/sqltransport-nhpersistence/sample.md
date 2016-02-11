---
title: Outbox - SQL Server Transport and NHibernate
summary: How to integrate SQL Server Transport with NHibernate persistence using Outbox
tags:
- SQL Server
- NHibernate
- Outbox
related:
- nservicebus/outbox
- nservicebus/sqlserver
- nservicebus/nhibernate
redirects:
- samples/sqltransport-nhpersistence-outbox
---

### Prerequisites
 1. Make sure you have SQL Server Express installed and accessible as `.\SQLEXPRESS`. 
 2. Create database called `nservicebus`.
 3. The [Outbox](/nservicebus/outbox) feature is designed to provide *exactly once* delivery guarantees without the Distributed Transaction Coordinator (DTC) running. Disable the DTC service to avoid seeing warning messages in the console window. If the DTC service is not disabled, you will see `DtcRunningWarning` message when the sample project is started. 

### Running the project
 1. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 2. In the Sender's console window you should see `Press <enter> to send a message` text.
 3. Start the Receiver project (right-click on the project, select the `Debug > Start new instance` option).
 4. In the Sender's console window you should see subscription confirmation `Subscribe from Receiver on message type OrderSubmitted`. 
 5. Hit `<enter>` to send a new message.

### Verifying that the sample works correctly
 1. On the Receiver console you should see information that an order was submitted.
 2. On the Sender console you should see information that the order was accepted.
 3. After a couple of seconds, on the Receiver console you should see information about receiving timeout message.
 4. Open SQL Server Management Studio and go to the `nservicebus` database. Verify that there is a row in saga state table (`dbo.OrderLifecycleSagaData`) and in the orders table (`dbo.Orders`).
 5. Verify that there are messages in the `dbo.audit` table and, if any message failed processing, messages in `dbo.error` table.

NOTE: The handling code has built-in chaotic behavior. There is a 50% chance that a given message fails processing. This is to demonstrate how error handling works. Since both First-Level Retries (FLR) and Second-Level Retires (SLR) are turned off, the message that couldn't be processed is immediately moved to the configured error queue.

To disable retries use the following snippets:
snippet:RetiresConfigurationXml
snippet:RetriesConfiguration

## Code walk-through

This sample contains three projects:

 * Shared - A class library containing common code including messages definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the `OrderSubmitted` message, sending `OrderAccepted` message and randomly generating exceptions.

### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQL Server transport with NHibernate persistence and Outbox.

Connection strings in version 2.x are passed using app.config. The version 3.x sample demonstrates how to specify them using code API.

snippet:SenderConnectionStrings
snippet:SenderConfiguration

### Receiver project

The Receiver mimics a back-end system. It is also configured to use SQL Server transport with NHibernate persistence and Outbox.

snippet:ReceiverConfiguration

In order for the Outbox to work, the business data has to reuse the same connection string as NServiceBus persistence:

snippet:NHibernate

When the message arrives at the Receiver, it is dequeued using a native SQL Server transaction. Then a `TransactionScope` is created that encompasses
 * persisting business data,

snippet:StoreUserData

 * persisting saga data of `OrderLifecycleSaga` ,
 * storing the reply message and the timeout request in the outbox.

snippet:Reply

snippet:Timeout

Finally the messages in the Outbox are pushed to their destinations. The timeout message gets stored in NServiceBus timeout store and is sent back to the saga after requested delay of 5 seconds.


## How it works

All the data manipulations happen atomically because SQL Server 2008 and later allows multiple (but not overlapping) instances of `SqlConnection` to enlist in one `TransactionScope` without the need to escalate to DTC. The SQL Server manages these transactions like they were one `SqlTransaction`.
