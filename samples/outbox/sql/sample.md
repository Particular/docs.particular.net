---
title: Using Outbox with SQL Server
summary: A sample demonstrating the SQL Server transport with SQL Persistence and ADO.NET user data store using outbox
reviewed: 2021-03-19
component: Core
related:
 - transports/sql
 - persistence/sql
 - nservicebus/outbox
redirects:
 - samples/outbox/sqltransport-sqlpersistence-ef
 - samples/outbox/sqltransport-nhpersistence-ef
 - samples/outbox/sqltransport-nhpersistence
---


Integrates the [SQL Server transport](/transports/sql) with [SQL persistence](/persistence/sql/) and ADO.NET user data store using outbox.

## Prerequisites

include: sql-prereq

The database created by this sample is `NsbSamplesSqlOutbox`.

The [outbox](/nservicebus/outbox) feature is designed to provide *exactly once* delivery guarantees without the [Distributed Transaction Coordinator (DTC)](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ms684146(v=vs.85)) running. Disable the DTC service to avoid seeing warning messages in the console window. If the DTC service is not disabled, when the sample project is started it will display a `DtcRunningWarning` message in the console window.

## Running the project

 1. Start the Solution
 1. The text `Press <enter> to send a message` is displayed in the Sender's console window.
 1. Press <kbd>enter</kbd> to send a new message.

## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.
 1. After a few seconds, the Receiver displays confirmation that the timeout message has been received.
 1. Open SQL Server Management Studio and go to the receiver database. Verify that there is a row in the saga state table (`Samples.SqlOutbox.Receiver.OrderLifecycleSaga`) and in the orders table (`receiver.SubmittedOrder`)

## Code walk-through

This sample contains three projects:

* Shared - A class library containing common code including the message definitions.
* Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
* Receiver - A console application responsible for processing the order message.

Sender and Receiver use different schemas in the same database. Apart from business data the database also contains tables representing queues for the NServiceBus endpoint and tables for NServiceBus persistence.

### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use the [SQL Server transport](/transports/sql/) with [SQL persistence](/persistence/sql/) (backed by SQL Server) and the outbox feature.

snippet: SenderConfiguration

### Receiver project

The Receiver mimics a back-end system. It is also configured to use the SQL Server transport with SQL persistence and the outbox. It uses [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview) to store business data (orders).

snippet: ReceiverConfiguration

When the message arrives at the Receiver, it is dequeued using a native SQL Server transaction. Then a separate Outbox SQL Server transaction is created that encompasses

* persisting business data:

snippet: StoreUserData

* persisting saga data of `OrderLifecycleSaga` ,
* storing the reply message and the timeout request in the Outbox:

snippet: Reply

snippet: Timeout

Once the outbox transaction is committed, both the business data changes and the outgoing messages are durably stored in the database. Finally the messages in the outbox are pushed to their destinations. The timeout message gets stored in the NServiceBus timeout store and is sent back to the saga after the requested delay of 5 seconds.

See [Accessing the ambient database details](/samples/sqltransport-sqlpersistence/#receiver-project-accessing-the-ambient-database-details) for information about using other ORMs.
