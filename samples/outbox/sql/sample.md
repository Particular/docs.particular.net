---
title: Using Outbox with SQL Server
summary: A sample demonstrating the SQL Server transport with SQL Persistence and ADO.NET user data store using outbox
reviewed: 2025-05-30
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

Demonstrates integration of the [SQL Server transport](/transports/sql) with [SQL persistence](/persistence/sql/) and an ADO.NET user data store using the outbox feature.

## Prerequisites

include: sql-prereq

This sample uses the `NsbSamplesSqlOutbox` database.

The [outbox](/nservicebus/outbox) feature ensures *exactly-once* delivery guarantees without requiring the [Distributed Transaction Coordinator (DTC)](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ms684146(v=vs.85)). To prevent warnings in the console, disable the DTC service. If not disabled, a `DtcRunningWarning` will appear when starting the sample.

## Running the project

1. Start the solution.
1. The Sender console displays `Press <enter> to send a message`.
1. Press <kbd>enter</kbd> to send a new message.

## Verifying correct behavior

1. The Receiver logs that an order was submitted.
1. The Sender logs that the order was accepted.
1. After a few seconds, the Receiver logs that the timeout message was received.
1. Open SQL Server Management Studio and check the `NsbSamplesSqlOutbox` database:
   * One row should exist in the saga state table (`receiver.OrderLifecycleSaga`).
   * One row should exist in the orders table (`receiver.SubmittedOrder`).

## Code walk-through

This sample includes three projects:

* **Shared** — Contains shared types such as message definitions.
* **Sender** — A console app that sends the initial `OrderSubmitted` message and handles the follow-up `OrderAccepted`.
* **Receiver** — A console app that handles incoming order messages.

Sender and Receiver use different schemas within the same database. The database includes business data, NServiceBus queue tables, and persistence tables.

### Sender project

The Sender does not persist data. It simulates a front-end system that submits orders, which are passed to the back-end via the bus. It's configured to use:

* [SQL Server transport](/transports/sql/)
* [SQL persistence](/persistence/sql/)
* Outbox support

snippet: SenderConfiguration

### Receiver project

The Receiver simulates a back-end system and is also configured with SQL Server transport, SQL persistence, and the outbox. It uses [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview) to store business data (orders).

snippet: ReceiverConfiguration

When a message arrives:

1. It's dequeued using a native SQL Server transaction.
2. A separate Outbox SQL transaction begins


This transaction codes:

* Business data persistence:

snippet: StoreUserData

* `OrderLifecycleSaga` saga state persistence
* Storing the reply and timeout messages in the outbox:

snippet: Reply

snippet: Timeout

Once the outbox transaction commits, both business data and outgoing messages are durably persisted. The outbox messages are then dispatched. The timeout message is stored in the NServiceBus timeout table and sent back to the saga after a 5-second delay.

For use with other ORMs, see [Accessing the ambient database details](/samples/sqltransport-sqlpersistence/#receiver-project-accessing-the-ambient-database-details).
