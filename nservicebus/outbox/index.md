---
title: Outbox
summary: Reliable messaging without distributed transactions
reviewed: 2018-03-06
component: Core
versions: "[5,)"
tags:
- DTC
redirects:
- nservicebus/no-dtc
related:
- samples/outbox
- persistence/nhibernate/outbox
- persistence/service-fabric/outbox
- persistence/ravendb/outbox
- persistence/sql/outbox
---

Outbox is an infrastructure feature which simulates the reliability of distributed transactions without requiring use of the Distributed Transaction Coordinator (DTC).

The DTC is used by NServiceBus to guarantee consistency between messaging operations and data persistence. Messaging operations include the receipt and successful processing of an incoming message, and the sending of any outgoing messages as part of that processing. Data persistence includes any business data persisted by a message handler, as well as any NServiceBus saga or timeout data stored at the same time. The DTC ensures that these operations either all complete successfully or are all rolled back.

The outbox feature can be used instead of the DTC to mimic the same level of consistency without using distributed transactions. This is done by storing outgoing messages in the database using the same local (non-distributed) transaction which is used to store business and NServiceBus data. After that transaction is successfully committed, the stored outgoing messages are dispatched to their destinations as a separate operation.


## How it works

The outbox feature is implemented using the [outbox](http://gistlabs.com/2014/05/the-outbox/) and the [deduplication](https://en.wikipedia.org/wiki/Data_deduplication#In-line_deduplication) patterns.

Every time an incoming message is processed, a copy of that message is stored in the persistent _outbox storage_. Whenever a new message is received, the framework determines whether that message has been processed already by checking for its presence in outbox storage.

If the message is not found in outbox storage then it is processed in the regular way, shown in the following diagram:

![No DTC Diagram](outbox.svg)

Processing a new incoming message consists of the following steps:

 * The handlers responsible for processing the message are invoked.
 * The _downstream messages_ (messages sent while processing the message, e.g. from inside handlers) are stored in outbox storage and business data is saved. Both operations are executed within a single (non-distributed) transaction.
 * The downstream messages are sent and then marked as _dispatched_ in outbox storage.

If an incoming message is found in outbox storage, then it is treated as a duplicate and is not processed again.

Even though an incoming message has been processed and business data has been saved, the framework might fail to immediately send the downstream messages. For example, the message queueing system may be unavailable, or the NServiceBus endpoint may be stopped before the messages can be dispatched. However, whenever an NServiceBus endpoint is started or is already running, and the framework detects downstream messages in outbox storage which are not marked as dispatched, it will attempt to dispatch them at that time.

Note: On the wire level, the outbox guarantees `at-least-once` message delivery, outgoing messages may be sent and processed multiple times. The outbox guarantees only transactionally exact-once processing, identical to distributed transactions, for all changes on the outbox storage transaction.


## Important design considerations

WARNING: Because the outbox uses a single (non-distributed) database transaction to store all data, the business data and outbox storage *must exist in the same database*.

 * The outbox feature works only for messages sent from NServiceBus message handlers.
 * Endpoints using DTC can only communicate with endpoints using the outbox if either of the following conditions are satisfied:
   * Endpoints using the outbox do not send messages to endpoints using DTC. However, endpoints using DTC can send messages to endpoints using the outbox.
   * If endpoints using the outbox send messages to endpoints using DTC, then the handlers processing those messages are [idempotent](https://en.wikipedia.org/wiki/Idempotence).
 * The outbox may generate duplicate messages if outgoing messages are successfully dispatched but the _Mark as dispatched_ phase fails. This may happen for a variety of reasons, including _outbox storage_ connectivity issues and deadlocks.


## Enabling the outbox

partial: enable-outbox

To learn about outbox configuration options such as time to keep deduplication data, or deduplication data clean up interval, refer to the dedicated pages for [NHibernate](/persistence/nhibernate/outbox.md), [RavenDB](/persistence/ravendb/outbox.md) or [ServiceFabric](/persistence/service-fabric/outbox.md) persistence.


## Converting from DTC to outbox

When converting a system from using the DTC to the outbox, care must be taken to ensure the system does not process duplicate messages incorrectly.

Because the outbox feature uses an "at least once" consistency guarantee at the transport level, endpoints that enable the outbox may occasionally send duplicate messages. These duplicate messages will be properly handled by deduplication in other Outbox-enabled endpoints, but will be processed more than once by endpoints which still use the DTC.

In order to gradually convert an entire system from the DTC to the outbox:

1. Enable the outbox on any endpoints that receive messages but do not send or publish any messages.
1. Enable the outbox on any endpoints that only send or publish messages to already-converted endpoints, where the outbox will be able to properly handle any duplicate messages.
1. Progress outward until all endpoints are converted.

WARNING: When verifying outbox functionality, it can sometimes be helpful to temporarily [stop the MSDTC Service](https://technet.microsoft.com/en-us/library/cc770732.aspx). This ensures that the outbox is working as expected, and no other resources are enlisting in distributed transactions.


## Message identity

The outbox only uses the incoming [message identifier](/nservicebus/messaging/message-identity.md) as a unique key for deduplicating messages. If the sender does not use outbox when sending messages, it is responsible for ensuring that the message identifier value is consistent when that message is sent multiple times.


## Outbox expiration duration

To determine if a message has been processed before, the identification data for each outbox record is retained. The duration that this data is retained for will vary depending on the persistence chosen for the outbox. The default duration, as well as the frequency of data removal, can be overriden for all outbox persistences.

After the outbox data retention period has lapsed, a retried message will be seen as the first of its kind and will be reprocessed. It is important to ensure that the retention period of outbox data is longer than the maximum time the message can be retried, including delayed retries and manual retries via ServicePulse.

Depending on the throughput of the system's endpoints, the outbox cleanup interval may need to be run more frequently. Increased frequency will allow each cleanup operation to purge the fewest records possible each time it runs. Purging fewer records will make the purge operation run faster which will ensure that it completes before the next purge operation is due to start.


## Storage requirements

The amount of storage space required for the outbox can be calculated as follows:

    Total outbox records = Message througput per second * Deduplication period in seconds

A single outbox record, after all transport operations have been dispatched, usually requires less than 50 bytes, most of which are taken for storing the original message ID as this is a string value.

NOTE: If the system is processing a high volume of messages, having a long deduplication timeframe will increase the amount of storage space that is required by outbox.

## Persistence

The outbox feature requires persistence in order to perform deduplication and to store outgoing downstream messages.

For more information on the outbox persistence options available refer to the dedicated persistence pages:

- [NHibernate](/persistence/nhibernate/outbox.md)
- [RavenDB](/persistence/ravendb/outbox.md)
- [SQL](/persistence/sql/outbox.md)
- [ServiceFabric](/persistence/service-fabric/outbox.md)
