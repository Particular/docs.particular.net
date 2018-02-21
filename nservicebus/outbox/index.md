---
title: Outbox
summary: Reliable messaging without distributed transactions
reviewed: 2016-08-03
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

The Outbox is an infrastructure feature which simulates the reliability of distributed transactions without requiring use of the Distributed Transaction Coordinator (DTC).

The DTC is used by NServiceBus to guarantee consistency between messaging operations and data persistence. Messaging operations include the receipt and successful processing of an incoming message, and the sending of any outgoing messages as part of that processing. Data persistence includes any business data persisted by a message handler, as well as any NServiceBus saga or timeout data stored at the same time. The DTC ensures that these operations either all complete successfully or are all rolled back.

The Outbox feature can be used instead of the DTC, to mimic the same level of consistency without using distributed transactions. This is done by storing outgoing messages in the database using the same local (non-distributed) transaction which is used to store business and NServiceBus data. After that transaction is successfully committed, the stored outgoing messages are dispatched to their destinations as a separate operation.


## How it works

The Outbox feature is implemented using the [outbox](http://gistlabs.com/2014/05/the-outbox/) and the [deduplication](https://en.wikipedia.org/wiki/Data_deduplication#In-line_deduplication) patterns.

Every time an incoming message is processed, a copy of that message is stored in the persistent _outbox storage_. Whenever a new message is received, the framework determines whether that message has been processed already by checking for its presence in outbox storage.

If the message is not found in outbox storage then it is processed in the regular way, shown in the following diagram:

![No DTC Diagram](outbox.svg)

Processing a new incoming message consists of the following steps:

 * The handlers responsible for processing the message are invoked.
 * The _downstream messages_ (messages sent while processing the message, e.g. from inside handlers) are stored in outbox storage and business data is saved. Both operations are executed within a single (non-distributed) transaction.
 * The downstream messages are sent and then marked as _dispatched_ in outbox storage.

If an incoming message is found in outbox storage, then it is treated as a duplicate and is not processed again.

Even though an incoming message has been processed and business data has been saved, the framework might fail to immediately send the downstream messages. For example, the message queueing system may be unavailable, or the NServiceBus endpoint may be stopped before the messages can be dispatched. However, whenever an NServiceBus endpoint is started or is already running, and the framework detects downstream messages in outbox storage which are not marked as dispatched, it will attempt to dispatch them at that time.

Note: On the wire level, the Outbox guarantees `at-least-once` message delivery, meaning downstream messages may be sent and processed multiple times. At the handler level, however, the Outbox guarantees only transactionally exact-once or non-transactional at-least-once message processing, identical to distributed transactions. This higher guarantee level is due to the deduplication that happens on the receiving side.


## Important design considerations

WARNING: Because the Outbox uses a single (non-distributed) database transaction to store all data, the business data and outbox storage *must exist in the same database*.

 * The Outbox feature works only for messages sent from NServiceBus message handlers.
 * Endpoints using DTC can only communicate with endpoints using the Outbox if either of the following conditions are satisfied:
   * Endpoints using the Outbox do not send messages to endpoints using DTC. However, endpoints using DTC can send messages to endpoints using the Outbox.
   * If endpoints using the Outbox send messages to endpoints using DTC, then the handlers processing those messages are [idempotent](https://en.wikipedia.org/wiki/Idempotence).
 * The Outbox may generate duplicate messages if outgoing messages are successfully dispatched but the _Mark as dispatched_ phase fails. This may happen for a variety of reasons, including _outbox storage_ connectivity issues and deadlocks.


## Enabling the Outbox

partial: enable-outbox

To learn about Outbox configuration options such as time to keep deduplication data, or deduplication data clean up interval, refer to the dedicated pages for [NHibernate](/persistence/nhibernate/outbox.md), [RavenDB](/persistence/ravendb/outbox.md) or [ServiceFabric](/persistence/service-fabric/outbox.md) persistence.


## Converting from DTC to Outbox

When converting a system from using the DTC to the Outbox, care must be taken to ensure the system does not process duplicate messages incorrectly.

Because the Outbox feature uses an "at least once" consistency guarantee at the transport level, endpoints that enable the Outbox may occasionally send duplicate messages. These duplicate messages will be properly handled by deduplication in other Outbox-enabled endpoints, but will be processed more than once by endpoints which still use the DTC.

In order to gradually convert an entire system from the DTC to the Outbox:

1. Enable the Outbox on any endpoints that receive messages but do not send or publish any messages.
1. Enable the Outbox on any endpoints that only send or publish messages to already-converted endpoints, where the Outbox will be able to properly handle any duplicate messages.
1. Progress outward until all endpoints are converted.

WARNING: When verifying outbox functionality, it can sometimes be helpful to temporarily [stop the MSDTC Service](https://technet.microsoft.com/en-us/library/cc770732.aspx). This ensures that the Outbox is working as expected, and no other resources are enlisting in distributed transactions.


## Message identity and idempotent processing

Using the Outbox might not be required if message handlers are idempotent. If the Outbox is needed, it will rely on [message identity](/nservicebus/messaging/message-identity.md) to perform deduplication. 

If an endpoint is configured to use Outbox, and it sends a message more than once, those messages will all have the same identifier value. The consistency of the identifier value across the duplicate messages is enforced by the Outbox.

If the endpoint does not use Outbox when sending messages, or if a message is sent outside of a handler, the code that is sending the message is responsible for ensuring that the identifier value is consistent when that message is sent multiple times. This will ensure that when the receiving handler's outbox instance sees the incoming messages it identifies them as being identical. At that point, the receiving outbox is able to deduplicate the identical messages.

Some transports have message deduplication built into them natively. This deduplication operates at the infrastructure level and doesn't rely out Outbox or any NServiceBus code. It is important to ensure that identity is managed by the code that is sending the messages. 

## Concurrency

 Outbox doesn't prevent concurrent attempts to process the same message. What Outbox can do is ensure that the outcome, when processing multiple copies of the same message, is persisted only once for resources that share the outbox transaction.

Processing of multiple messages can occur if there are multiple physical copies of a message in the queue and if there are multiple ways for messages to be processed at the same time. This will occur if and endpoint:
- has a concurrency setting greater than 1
- is scaled out and running as multiple processes

In either of these situations it is possible that multiple physical copies of the message will be retrieved from the queue and processed simultaneously.

## Non transactional resources

When accessing non-transactional resources, such as a REST endpoint, as part of a message handler it is important to ensure that message processing is idempotent. Outbox does not guarantee that it will handle non-transactional resources in an idempotent manner. To prevent more-than-once invocation of a non-transactional resource, configure the endpoint to have a [maximum concurrency of 1](/nservicebus/operations/tuning.md#tuning-concurrency).

NOTE: Invoking non-transactional resources multiple times is not a problem that is specific to Outbox. Its mentioned here since it is commonly assumed that outbox deduplication will prevent more-than-once invocation.

Try to avoid mixing transactional and non transactional tasks. If performing non transactional tasks send a message to perform this task in isolation.


## Outbox expiration duration

Part of the purpose of the Outbox is to guarantee that the data remains consistent if the same message is processed more than once. A message with the exact same identification can be detected and ignored. To determine if a message has been processed before, the identification data for each outbox record is retained. The duration that this data is retained for will vary depending on the persistence chosen for the Outbox. The default duration, as well as the frequency of data removal, can be overrode for all outbox persistences.

It is important to ensure that the retention period of outbox data is longer than the maximum time the message can be retried, including delayed retries and manual retries via ServiceControl. Additional care must be taken by operators of ServicePulse to not retry messages older than the Outbox retention period. If a message is processed, manually or automatically, after the outbox data retention period has lapsed, that message will be seen as the first of its kind and it will not be deduplicated.

Most outbox persisters run a cleanup task every minute, but the exact frequency varies depending on the persistence that is chosen.

Depending on the throughput of the system's endpoints, the outbox cleanup interval may need to be run more frequently. Increased frequency will allow each cleanup operation to purge the fewest records possible each time it runs. Purging fewer records will make the purge operation run faster which will ensure that it completes before the next purge operation is due to start.


## Storage requirements

The amount of storage space required for Outbox can be calculated as follows:

    Total outbox records = Message througput per second * Deduplication period in seconds

A single outbox record, after all transport operations have been dispatched, usually requires less than 50 bytes of which most are taken for storing the original message ID as this is a string value.

NOTE: If the system is processing a high volume of messages, having a long deduplication timeframe will increase the amount of storage space that is required by Outbox.

## Persistence

The Outbox feature requires persistence in order to perform deduplication and to store outgoing downstream messages.

For more information on the outbox persistence options available refer to the dedicated persistence pages:

- [NHibernate](/persistence/nhibernate/outbox.md)
- [RavenDB](/persistence/ravendb/outbox.md)
- [SQL](/persistence/sql/outbox.md)
- [ServiceFabric](/persistence/service-fabric/outbox.md)
