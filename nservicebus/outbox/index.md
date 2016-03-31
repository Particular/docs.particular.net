---
title: Outbox
summary: How to configure NServiceBus to provide reliable messaging without using MSDTC or when MSDTC is not available
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

In order to enable the Outbox for SQL Server and MSMQ transports, the following is required:

snippet: OutboxEnablingInAppConfig

NOTE: The double opt-in configuration for transports supporting DTC was introduced to make sure that Outbox is not used in combination with DTC accidentally. The Outbox can be used with DTC only if **all endpoints are [idempotent](https://en.wikipedia.org/wiki/Idempotence)**. Otherwise, problems could arise from double-processing messages sent more than once (because of at-least-once guarantee).


## How it works

These feature has been implemented using both the [Outbox pattern](http://gistlabs.com/2014/05/the-outbox/) and the [Deduplication pattern](https://en.wikipedia.org/wiki/Data_deduplication#In-line_deduplication).

When a message is dequeued it is checked for previous processing attempts. If the message has already been processed and all downstream messages are marked as dispatched, the current message is ignored (considered a duplicate). If the downstream messages are not marked as dispatched (they may or may not have been sent), they are re-sent and marked as dispatched. Otherwise (if the message has not yet been processed) the regular handler logic is invoked storing all outgoing message in a durable storage in the same transaction as the users own database changes. Finally, all outgoing downstream messages are sent and marked as dispatched.

Here is a diagram illustrating how it works:

![No DTC Diagram](outbox.png)


## Caveats

 * Both the business data and deduplication data need to share the same database.
 * The Outbox is bypassed when a message is sent outside of an NServiceBus message handler. This is because there is no mechanism for repeated dispatching of the Outbox messages other than the standard retry logic, which won't be triggered in this case.


## Using Outbox with NHibernate persistence


### SQL Server Transport

SQL Server transport in combination with NHibernate persistence can support *exactly-once* message delivery in two ways:

 * by sharing the database connection, which is enlisted in TransactionScope;
 * by using Outbox.

If Outbox is enabled the messages are stored in the same physical store as saga and user data. The messages are dispatched only after the processing is finished. When NHibernate persistence detects that Outbox is enabled and used together with SQLServer transport, then it automatically stops reusing the transport connection and transaction. All the data access is done within the Outbox ambient transaction. 

From the perspective of a particular endpoint this is *exactly-once* processing because of the deduplication that happens on the incoming queue. From a global point of view this is *at-least-once* since on the wire messages can get duplicated.
 
The latter is discussed in depth in the [Outbox - SQL Transport and NHibernate](/samples/sqltransport-nhpersistence) sample.


### What extra tables does NHibernate Outbox persistence create

To keep track of duplicate messages, the NHibernate implementation of Outbox requires the creation of two additional tables in the database: `OutboxRecord` and `OutboxOperation`.


### How long are the deduplication records kept

The NHibernate implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the config file using [TimeStamp strings](https://msdn.microsoft.com/en-us/library/ee372286.aspx):

snippet:OutboxNHibernateTimeToKeep

## Using Outbox with RavenDB persistence


### What extra documents does RavenDB outbox persistence create

To keep track of duplicate messages, the RavenDB implementation of Outbox creates a special collection of documents called `OutboxRecord`.


### How long are the deduplication records kept

The RavenDB implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the settings dictionary:

snippet:OutboxRavendBTimeToKeep

## Configuring outbox


### Cleanup interval


An endpoint that processes a high volume of messages per second requires a shorter interval to clean up more frequently.

For example, an endpoint that processes 1,000 messages per second generates 60,000 outbox records per minute. In that case, running the cleanup task every second would result in deleting 1,000 records each interval. Running the cleanup task every 100 milliseconds reduces this number to 100 records each run.

### Idempotency

If the application is idempotent then deduplication is not required. The outbox is then only used to deliver messages in an at-least-once behavior. In such case the deduplication time window could even be set to expire immediately.

Outbox records will only be removed when all the containing transport operations are succeeeded.

### Deduplication time window

There is no exact calculation that yields seven days as a magic number. What needs to be considered is: how fast will an issue be noticed and fixed that can lead to duplication of messages?

#### Server failure

How fast will it be noticed that a sending system goes down between the two operations because of hardware issues? Will this system be recovered in minutes, hours or days? Most likely not minutes unless its a high available cluster, maybe hours when there is a good recovery strategy, but most likely within seven days.

#### Network failure

Network failures like delayed acks and timeout are even harder to resolve due to routing and congestion. Such issues often take a long time often requiring external experts taking days to resolve.

#### OPS Detect, asses and fix period

Minutes or hours is usually too short a period to detect, assess and fix a scenario as mentioned. In seven days it is likely to have detected that something is wrong and resolve it. Having a available infrastructure and a 24x7 expert DevOps team creates the possibility to reduce the deduplication window from 7 days accordingly.

#### Increase time window in case of issues

If an issue is detected and more time is needed to resolve it, there is always the possibility to *increase* the deduplication window on the receivers to create more time to come up with a fix.

#### Adjusting deduplication time window

A large set of deduplication records might need to be deleted when reverting the deduplication time window to the old value. 

For example, the deduplication time window is 1 day. An issue occurs, the ops team sets it to one week. The issue is resolved after 2,5 days including detection and assesment, instead of reverting to 1 day it probably is best to set it to 2,5 days. The 1,5 days of recorded deduplication data will not be purged at the next cleanup interval which can potentially cause congestion and locking issues.


