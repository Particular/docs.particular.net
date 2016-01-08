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

In order to enable the Outbox:

snippet: OutboxEnablineInCode

This is enough for the RabbitMQ transport. For SQL Server and MSMQ transports, to enable the outbox users need to also explicitly set the following configuration settings when configuring the endpoint:

snippet: OutboxEnablingInAppConfig

NOTE: It may seem extreme to require double opt-in configuration of the Outbox for all other transports, but this is because we want users to be very aware that this new feature should not be used with existing endpoints that currently use DTC, unless those endpoints are [idempotent](https://en.wikipedia.org/wiki/Idempotence). Otherwise, problems could arise from double-processing messages sent (via an at-least-once guarantee) more than once.


## How does it work

These feature has been implemented using both the [Outbox pattern](http://gistlabs.com/2014/05/the-outbox/) and the [Deduplication pattern](https://en.wikipedia.org/wiki/Data_deduplication#In-line_deduplication).

As a message is dequeued we check to see if we have previously processed it. If so, we then deliver any messages in the outbox for that message but do not invoke message-processing logic again. If the message wasn't previously processed, then we invoke the regular handler logic, storing all outgoing message in a durable storage in the same transaction as the users own database changes. Finally we send out all outgoing messages and update the deduplication storage.

Here is a diagram how it all works:

![No DTC Diagram](outbox.png)


## Caveats

- Both the business data and deduplication data need to share the same database.
- The Outbox is bypassed when sending messages "from outside" via the `IBus` interface (not from a message handler). The reason for this is lack of a driving force for repeated dispatching of the Outbox messages (which currently is the retry mechanism that applies only when handling messages).

## Using outbox with NHibernate persistence


### SQL Server Transport

SQL Server transport supports *exactly-once* message delivery without Outbox solely by means of sharing the transport connection with persistence. This mode of operation is discussed in depth in this [sample](/samples/sqltransport-nhpersistence).


### What extra tables does NHibernate outbox persistence create

To keep track duplicate messages, the NHibernate implementation of Outbox requires the creation of two additional tables in your database, these are called `OutboxRecord` and `OutboxOperation`.


### How long are the deduplication records kept

The NHibernate implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the config file using [TimeStamp strings](https://msdn.microsoft.com/en-us/library/ee372286.aspx), here is how to do it:

snippet:OutboxNHibernateTimeToKeep


## Using outbox with RavenDB persistence


### What extra documents does RavenDB outbox persistence create

To keep track duplicate messages, the RavenDB implementation of Outbox creates a special collection of documents inside your database, these are called `OutboxRecord`.


### How long are the deduplication records kept

The RavenDB implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the settings dictionary, here is how to do it:

snippet:OutboxRavendBTimeToKeep
