---
title: Outbox
summary: 'How to configure NServiceBus to provide reliable messaging without using MSDTC or when MSDTC is not available'
tags:
- MSDTC
redirects:
 - nservicebus/no-dtc
related:
 - samples/outbox
---

NServiceBus Version 5 adds the option of running endpoints with similar reliability to DTC while not actually using DTC.


## Enabling the outbox

In order to enable the Outbox for RabbitMQ transport:

snippet: OutboxEnablineInCode

In order to enable the Outbox for SQL Server and MSMQ transports, you additionally need:

snippet: OutboxEnablingInAppConfig

NOTE: The double opt-in configuration for transports supporting DTC was introduced to make sure that Outbox is not used in combination with DTC accidentally. The Outbox can be used with DTC only if **all endpoints are [idempotent](https://en.wikipedia.org/wiki/Idempotence)**. Otherwise, problems could arise from double-processing messages sent more than once (because of at-least-once guarantee).

## How it works

These feature has been implemented using both the [Outbox pattern](http://gistlabs.com/2014/05/the-outbox/) and the [Deduplication pattern](https://en.wikipedia.org/wiki/Data_deduplication#In-line_deduplication).

When a message is dequeued we check if we have processed it already. If so, we then deliver any messages in the outbox for that message but do not invoke message-processing logic again. If the message hasn't been processed, then we invoke the regular handler logic, storing all outgoing message in a durable storage in the same transaction as the users own database changes. Finally we send out all outgoing messages and update the deduplication storage.

Here is a diagram illustrating how it works:

![No DTC Diagram](outbox.png)

## Caveats

- Both the business data and deduplication data need to share the same database.
- The Outbox is bypassed when a message is sent outside of an NServiceBus message handler. This is because there is no mechanism for repeated dispatching of the Outbox messages other than the standard retry logic, which won't be triggered in this case.

## Using Outbox with NHibernate persistence

### SQL Server Transport

SQL Server transport supports *exactly-once* message delivery without Outbox solely by means of sharing the transport connection with persistence. This mode of operation is discussed in depth in this [sample](/samples/sqltransport-nhpersistence).

### What extra tables does NHibernate Outbox persistence create

To keep track of duplicate messages, the NHibernate implementation of Outbox requires the creation of two additional tables in the database: `OutboxRecord` and `OutboxOperation`.

### How long are the deduplication records kept

The NHibernate implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the config file using [TimeStamp strings](https://msdn.microsoft.com/en-us/library/ee372286.aspx):

snippet:OutboxNHibernateTimeToKeep


## Using Outbox with RavenDB persistence


### What extra documents does RavenDB outbox persistence create

To keep track of duplicate messages, the RavenDB implementation of Outbox creates a special collection of documents inside your database called `OutboxRecord`.

### How long are the deduplication records kept

The RavenDB implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the settings dictionary:

snippet:OutboxRavendBTimeToKeep
