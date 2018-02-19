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

The Outbox feature is implemented using the [Outbox](http://gistlabs.com/2014/05/the-outbox/) and the [Deduplication](https://en.wikipedia.org/wiki/Data_deduplication#In-line_deduplication) patterns.

Every time an incoming message is processed, a copy of that message is stored in the persistent _Outbox storage_. Whenever a new message is received, the framework determines whether that message has been processed already by checking for its presence in Outbox storage.

If the message is not found in Outbox storage then it is processed in the regular way, shown in the following diagram:

![No DTC Diagram](outbox.svg)

Processing a new incoming message consists of the following steps:

 * The handlers responsible for processing the message are invoked.
 * The _downstream messages_ (messages sent while processing the message, e.g. from inside handlers) are stored in Outbox storage and business data is saved. Both operations are executed within a single (non-distributed) transaction.
 * The downstream messages are sent and then marked as _dispatched_ in Outbox storage.

If an incoming message is found in Outbox storage, then it is treated as a duplicate and is not processed again.

Even though an incoming message has been processed and business data has been saved, the framework might fail to immediately send the downstream messages. For example, the message queueing system may be unavailable, or the NServiceBus endpoint may be stopped before the messages can be dispatched. However, whenever an NServiceBus endpoint is started or is already running, and the framework detects downstream messages in Outbox storage which are not marked as dispatched, it will attempt to dispatch them at that time.

Note: On the wire level, the Outbox guarantees `at-least-once` message delivery, meaning downstream messages may be sent and processed multiple times. At the handler level, however, the Outbox guarantees only transactionally exact-once or non-transactional at-least-once message processing, identical to distributed transactions. This higher guarantee level is due to the deduplication that happens on the receiving side.


## Important design considerations

WARNING: Because the Outbox uses a single (non-distributed) database transaction to store all data, the business data and Outbox storage *must exist in the same database*.

 * The Outbox feature works only for messages sent from NServiceBus message handlers.
 * Endpoints using DTC can only communicate with endpoints using the Outbox if either of the following conditions are satisfied:
   * Endpoints using the Outbox do not send messages to endpoints using DTC. However, endpoints using DTC can send messages to endpoints using the Outbox.
   * If endpoints using the Outbox send messages to endpoints using DTC, then the handlers processing those messages are [idempotent](https://en.wikipedia.org/wiki/Idempotence).
 * The Outbox may generate duplicate messages if outgoing messages are successfully dispatched but the _Mark as dispatched_ phase fails. This may happen for a variety of reasons, including _Outbox storage_ connectivity issues and deadlocks.


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

If handlers are idempotent then outbox might not be required at all. Outbox relies on message identity to guarantee consistency. When using outbox on both the sender and receiver message identifiers are consistent if a handler is invoked more-than-once for a message. 
If a sender does not use the outbox or when sending a message outside of a handler the sender *must* generate and apply the same message identifier to guarantee consistency at the receiver.
The same message identity is also important to have deduplication working on the infrastructure level on some transport that support this. [Read more on how to set a message identifier.](/nservicebus/messaging/message-identity.md)


## Concurrency

Outbox cannot guarantee that handlers or sagas are invoked exactly once. Outbox does not prevent concurrent attempts to process the same message but ensures that the outcome of the processing is persisted only once for resources that share the outbox transaction.

The following scenarios might cause handlers to be invoked multiple times:

- When a sender sends multiple physical message with the same message identifier. An example is a order form which is submitted multiple times.
- When a transport performs a failover and operates in a at-least-once delivery model and has not marked a message as processed while this is the case but the acknowledgement was not processed by the tranport yet.
- When a failure occurs during the outbox dispatch stage. The dispatch stage will to be invoked more-than-once and each time message can message can partially be delivered to the transport infrastructure.

In the above scenarios, multiple physical copies of the same logical message exist in the queue. If an endpoint allows concurrent message processing, or it is scaled out, its possible that the multiple copies of the message can be processed concurrently.

## Non transactional resources

If non-transactional resources are being accessed then make sure processing is idempotent. Non-transactional resources are not guaranteed to be handled in an idempotent way by the Outbox. If more-than-once invocation is not wanted, then this can be prevented almost completely by making sure that only a single endpoint instance is running and having [configured that endpoint with a maximum concurrency of 1](/nservicebus/operations/tuning.md#tuning-concurrency). Only data modified via a shared storage session is made idempotent via the outbox and has exact-once semantics. See [persistence specific documention](/nservicebus/outbox/#Persistence) on how to obtain the shared storage session.

NOTE: This problem is not specific to Outbox but mentioned here as sometimes it is assumed Outbox does deduplication that prevents more-than-once invocation.

Try to avoid mixing transactional and non transactional tasks. If performing non transactional tasks send a message to perform this task in isolation.


## Outbox expiration duration

Part of the purpose of the outbox is to guarantee that the data remains consistent if the same message is processed more than once. A message with the exact same identification can be detected and ignored. The identification data for each outbox record is retained. The default duration that the data is retained for will vary depending on the persistence chosen for the outbox. The default duration, as well as the frequency of data removal, can be overrode for all outbox persistences.

Ensure that the retention period of the outbox is longer than the maximum time the message can be retried, including delayed retries and manual retries via ServiceControl. Additional care must be taken by operators of ServicePulse to not retry messages older than the outbox retention period.

## Required storage

Outbox requires storage. The amount of storage space required for Outbox can be calculated as follows:

    Total outbox records = Message througput per second * Deduplication period in seconds

A single outbox record, after all transport operations have been dispatched, usually requires less than 50 bytes of which most are taken for storing the original message ID as this is a string value.

NOTE: If the system is processing a high volume of messages, having a long deduplication timeframe will increase the amount of storage space that is required by Outbox.

## Outbox cleanup interval

Outbox cleanup varies depending on the persistence that is chosen. Most persisters run a cleanup task every minute.

Depending on the throughput of the system's endpoints, the Outbox cleanup interval can be run more frequently. This will allow each cleanup operation to purge the fewest records possible each time it runs, which will allow the cleanup to execute faster.


## Persistence

The Outbox feature requires persistence in order to perform deduplication and store outgoing downstream messages.

For more information refer to the dedicated persistence pages:

- [NHibernate](/persistence/nhibernate/outbox.md)
- [RavenDB](/persistence/ravendb/outbox.md)
- [SQL](/persistence/sql/outbox.md)
- [ServiceFabric](/persistence/service-fabric/outbox.md)
