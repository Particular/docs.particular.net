---
title: Outbox - SQL Transport and NHibernate
summary: Integrating SQL Transport Transport with NHibernate persistence using Outbox.
reviewed: 2016-03-21
component: SqlServer
tags:
- Outbox
related:
- nservicebus/outbox
- transports/sql
- persistence/nhibernate
redirects:
- samples/sqltransport-nhpersistence-outbox
---

## Prerequisites

 1. Make sure SQL Server Express is installed and accessible as `.\SQLEXPRESS`.
 1. Create database called `nservicebus`.
 1. The [Outbox](/nservicebus/outbox) feature is designed to provide *exactly once* delivery guarantees without the Distributed Transaction Coordinator (DTC) running. Disable the DTC service to avoid seeing warning messages in the console window. If the DTC service is not disabled, when the sample project is started it will display `DtcRunningWarning` message in the console window.


## Running the project

 1. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 1. The text `Press <enter> to send a message` should be displayed in the Sender's console window.
 1. Start the Receiver project (right-click on the project, select the `Debug > Start new instance` option).
 1. The Sender should display subscription confirmation `Subscribe from Receiver on message type OrderSubmitted`.
 1. Hit enter to send a new message.


## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.
 1. Finally, after a couple of seconds, the Receiver displays confirmation that the timeout message has been received.
 1. Open SQL Server Management Studio and go to the `nservicebus` database. Verify that there is a row in saga state table (`dbo.OrderLifecycleSagaData`) and in the orders table (`dbo.Orders`).
 1. Verify that there are messages in the `dbo.audit` table and, if any message failed processing, messages in `dbo.error` table.

NOTE: The handling code has built-in chaotic behavior. There is a 50% chance that a given message fails processing. This is to demonstrate how [recoverability](/nservicebus/recoverability/) works. Since recoverability is disabled, the message that couldn't be processed is immediately moved to the configured error queue.

The retries are disabled using the following settings:

partial: RetriesConfigurationXml

partial: RetriesConfiguration


## Code walk-through

This sample contains three projects:

 * Shared - A class library containing common code including messages definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the `OrderSubmitted` message, sending `OrderAccepted` message and randomly generating exceptions.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQL Server transport with NHibernate persistence and Outbox.

partial: SenderConfiguration


### Receiver project

The Receiver mimics a back-end system. It is also configured to use SQL Server transport with NHibernate persistence and Outbox.

snippet: ReceiverConfiguration

In order for the Outbox to work, the business data has to reuse the same connection string as NServiceBus persistence:

snippet: NHibernate

When the message arrives at the Receiver, it is dequeued using a native SQL Server transaction. Then a `TransactionScope` is created that encompasses

 * persisting business data:

snippet: StoreUserData

 * persisting saga data of `OrderLifecycleSaga`,
 * storing the reply message and the timeout request in the outbox:

snippet: Reply

snippet: Timeout

Finally the messages in the Outbox are pushed to their destinations. The timeout message gets stored in NServiceBus timeout store and is sent back to the saga after requested delay of 5 seconds.


## How it works

All the data manipulations happen atomically because all supported versions of SQL Server allow multiple (but not overlapping) instances of `SqlConnection` to enlist in a single `TransactionScope` without the need to escalate to DTC. The SQL Server manages these transactions like they were just one `SqlTransaction`.
