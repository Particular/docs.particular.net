---
title: Outbox - SQL Transport, NHibernate and EF Core
summary: Integrating SQL Transport with NHibernate persistence and Entity Framework Core user data store using outbox.
reviewed: 2016-03-21
component: SqlServer
tags:
- Outbox
related:
- persistence/nhibernate
- transports/sql
- nservicebus/outbox
redirects:
- samples/sqltransport-nhpersistence-outbox-ef
---


## Prerequisites

 1. Make sure SQL Server Express is installed and accessible as `.\SQLEXPRESS`.
 1. Create database called `nservicebus`.
 1. In the database create schemas `sender` and `receiver`.
 1. The [Outbox](/nservicebus/outbox) feature is designed to provide *exactly once* delivery guarantees without the Distributed Transaction Coordinator (DTC) running. Disable the DTC service to avoid seeing warning messages in the console window. If the DTC service is not disabled, when the sample project is started it will display `DtcRunningWarning` message in the console window.


## Running the project

 1. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 1. The text `Press <enter> to send a message` should be displayed in the Sender's console window.
 1. Start the Receiver project (right-click on the project, select the `Debug > Start new instance` option).
 1. Hit enter to send a new message.


## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.
 1. Finally, after a couple of seconds, the Receiver displays confirmation that the timeout message has been received.
 1. Open SQL Server Management Studio and go to the receiver database. Verify that there is a row in saga state table (`dbo.OrderLifecycleSagaData`) and in the orders table (`dbo.Orders`)


## Code walk-through

This sample contains three projects:

 * Shared - A class library containing common code including the message definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the order message.

Sender and Receiver use different schemas in the same database. Apart from business data the database also contains tables representing queues for the NServiceBus endpoint and tables for NServiceBus persistence.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQLServer transport with NHibernate persistence and Outbox.

snippet: SenderConfiguration

The Sender uses a configuration file to tell NServiceBus where the messages addressed to the Receiver should be sent:

snippet: SenderConnectionStrings


### Receiver project

The Receiver mimics a back-end system. It is also configured to use SQLServer transport with NHibernate persistence  and Outbox. It uses [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) to store business data (orders).

snippet: ReceiverConfiguration

In order for the Outbox to work, the business data has to reuse the same connection string as NServiceBus persistence:

snippet: EntityFramework

When the message arrives at the Receiver, it is dequeued using a native SQL Server transaction. Then a `TransactionScope` is created that encompasses

 * persisting business data:

snippet: StoreUserData

 * persisting saga data of `OrderLifecycleSaga` ,
 * storing the reply message and the timeout request in the Outbox:

snippet: Reply

snippet: Timeout

Finally the messages in the Outbox are pushed to their destinations. The timeout message gets stored in the NServiceBus timeout store and is sent back to the saga after requested delay of 5 seconds.


## How it works

All the data manipulations happen atomically because SQL Server 2008 and later allows multiple (but not overlapping) instances of `SqlConnection` to enlist in a single `TransactionScope` without the need to escalate to DTC. The SQL Server manages these transactions like they were just one `SqlTransaction`.
